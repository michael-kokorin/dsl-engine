namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Infrastructure.Plugins.Contracts;
	using Infrastructure.Telemetry;
	using Infrastructure.Telemetry.Entities;
	using Infrastructure.Templates;
	using Repository.Context;
	using Workflow.GitHub.Properties;
	using Workflow.VersionControl;

	[UsedImplicitly]
	internal sealed class SaveToItPostProcessingStage: PostProcessingStage
	{
		private readonly IBranchNameBuilder _branchNameBuilder;

		private readonly IIssueNameBuilder _issueNameBuilder;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		private readonly ITemplateProvider _templateProvider;

		public SaveToItPostProcessingStage(
			[NotNull] IBranchNameBuilder branchNameBuilder,
			[NotNull] IIssueNameBuilder issueNameBuilder,
			[NotNull] ITemplateProvider templateProvider,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider)
		{
			if(branchNameBuilder == null) throw new ArgumentNullException(nameof(branchNameBuilder));
			if(issueNameBuilder == null) throw new ArgumentNullException(nameof(issueNameBuilder));
			if(templateProvider == null) throw new ArgumentNullException(nameof(templateProvider));
			if(telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));

			_branchNameBuilder = branchNameBuilder;
			_issueNameBuilder = issueNameBuilder;
			_templateProvider = templateProvider;
			_telemetryScopeProvider = telemetryScopeProvider;
		}

		protected override Func<Tasks, bool> PreCondition => task => task.Projects.CommitToIt;

		protected override void ExecuteStage(PostProcessingBundle bundle)
		{
			var fpAnnotation =
				bundle.IssueVulnerabilityLinks
					.Select(x => x.IssueAnnotation)
					.Concat(bundle.RemainingIssues)
					.Where(x => x.State == IssueAnnotationState.FalsePositive)
					.ToArray();

			var fixedAnnotation = bundle.RemainingIssues
										.Concat(bundle.Changes.Where(x => x.Type == FileChangeAnnotationType.Update).Select(x => x.Annotation))
										.Where(x => x.State == IssueAnnotationState.Fixed)
										.ToArray();

			var todoVulnerabilities = bundle.Changes.Where(x => x.Annotation.State == IssueAnnotationState.Todo).ToArray();
			var reopenVulnerabilities = bundle.Changes.Where(x => x.Annotation.State == IssueAnnotationState.Reopen).ToArray();

			var groups =
				fpAnnotation.Select(x => new {Type = x.LongName, IssueId = x.Id})
							.Concat(
								fixedAnnotation.Select(
									x => new
										{
											Type = x.LongName,
											IssueId = x.Id
										}))
							.Concat(
								todoVulnerabilities.Concat(reopenVulnerabilities)
													.Select(x => new {Type = x.Annotation.LongName, IssueId = x.Annotation.Id}))
							.Distinct()
							.ToArray();

			foreach(var group in groups)
			{
				var branchName = _branchNameBuilder.Build(new BranchNameBuilderInfo {Type = group.Type});

				var template = _templateProvider.Get(TemplateNames.IssueBody);

				var fp = fpAnnotation.Where(x => x.LongName == group.Type).GroupBy(
										x => new
											{
												x.File,
												x.LineStart
											}).Select(x => x.First()).ToArray();

				var fixedItems = fixedAnnotation.Where(x => x.LongName == group.Type).GroupBy(
													x => new
														{
															x.File,
															x.LineStart
														}).Select(x => x.First()).ToArray();

				var todoItems = todoVulnerabilities
					.Where(x => x.Annotation.LongName == group.Type)
					.GroupBy(
						x => new
							{
								x.Vulnerability.File,
								x.Vulnerability.NumberLine
							}).Select(x => x.First()).ToArray();

				var reopenItems = reopenVulnerabilities
					.Where(x => x.Annotation.LongName == group.Type)
					.GroupBy(
						x => new
							{
								x.Vulnerability.File,
								x.Vulnerability.NumberLine
							}).Select(x => x.First()).ToArray();

				template.Body.Add(
							new Dictionary<string, object>
							{
								{
									"FP", fp
								},
								{
									"Fixed", fixedItems
								},
								{
									"Todo", todoItems
								},
								{
									"Reopen", reopenItems
								},

								// TODO: remove code block cause incorrect read out strongly typed GitHub settings
								//{
								//	"RepoOwner", settings[GitHubItSettingKeys.RepositoryOwner.ToString()]
								//},
								//{
								//	"RepoName", settings[GitHubItSettingKeys.RepositoryName.ToString()]
								//},
								{
									"RepoBranch", branchName
								},
								{
									"TotalCount", fp.Length + fixedItems.Length + todoItems.Length + reopenItems.Length
								}
							});

				var body = template.Body.Render();

				var issue = bundle.Issues.FirstOrDefault(x => x.Id == group.IssueId) ??
							bundle.IssueTrackerPlugin.GetIssue(group.IssueId);

				var title = _issueNameBuilder.Build(
					new IssueNameBuilderInfo
					{
						IssueTypeName = group.Type
					});

				if(fp.Any() || fixedItems.Any() || todoItems.Any() || reopenItems.Any())
					title += Resources.IssueNameLeft.FormatWith(todoItems.Length + reopenItems.Length);

				using(var telemetryScope = _telemetryScopeProvider.Create<ItPluginInfo>(TelemetryOperationNames.ItPlugin.Update))
				{
					try
					{
						telemetryScope.SetEntity(
							new ItPluginInfo
							{
								Plugins = bundle.Task.Projects.Plugins,
								TaskId = bundle.Task.Id
							});

						bundle.IssueTrackerPlugin.UpdateIssue(
								new UpdateIssueRequest
								{
									Id = issue.Id,
									Title = title,
									Description = body,
									Status =
										todoVulnerabilities.Concat(reopenVulnerabilities).Any(x => x.Annotation.LongName == group.Type)
											? IssueStatus.Open
											: IssueStatus.Closed
								});

						telemetryScope.WriteSuccess();
					}
					catch(Exception ex)
					{
						telemetryScope.WriteException(ex);

						throw;
					}
				}
			}
		}
	}
}