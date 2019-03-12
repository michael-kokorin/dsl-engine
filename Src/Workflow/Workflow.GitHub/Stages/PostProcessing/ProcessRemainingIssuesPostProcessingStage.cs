namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Contracts;
	using Infrastructure.Telemetry;
	using Infrastructure.Telemetry.Entities;
	using Repository.Context;

	[UsedImplicitly]
	internal sealed class ProcessRemainingIssuesPostProcessingStage: PostProcessingStage
	{
		private readonly IIssueNameBuilder _issueNameBuilder;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		public ProcessRemainingIssuesPostProcessingStage(
			[NotNull] IIssueNameBuilder issueNameBuilder,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider)
		{
			if(issueNameBuilder == null) throw new ArgumentNullException(nameof(issueNameBuilder));
			if(telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));

			_issueNameBuilder = issueNameBuilder;
			_telemetryScopeProvider = telemetryScopeProvider;
		}

		protected override Func<Tasks, bool> PreCondition => task => true;

		protected override void ExecuteStage(PostProcessingBundle bundle)
		{
			var vulnerabilityTypeGroups = bundle.VulnerabilitiesInfo
												.Where(x => bundle.IssueVulnerabilityLinks.All(y => y.VulnerabilityInfo != x))
												.GroupBy(x => x.Type);

			// new
			foreach(var vulnerabilityGroup in vulnerabilityTypeGroups)
			{
				var issueTitle = _issueNameBuilder.Build(
					new IssueNameBuilderInfo
					{
						IssueTypeName = vulnerabilityGroup.Key
					});

				Issue itIssue = null;

				if(bundle.Task.Projects.CommitToIt)
				{
					itIssue = bundle.Issues.FirstOrDefault(
										x =>
											x.Title.StartsWith(
												issueTitle,
												StringComparison.InvariantCultureIgnoreCase));

					if(itIssue == null)
					{
						using(var telemetryScope = _telemetryScopeProvider.Create<ItPluginInfo>(TelemetryOperationNames.ItPlugin.Create))
						{
							try
							{
								telemetryScope.SetEntity(
									new ItPluginInfo
									{
										Plugins = bundle.Task.Projects.Plugins,
										TaskId = bundle.Task.Id
									});

								itIssue = bundle.IssueTrackerPlugin.CreateIssue(
													bundle.Task.Repository,
													new CreateIssueRequest
													{
														Title = issueTitle,
														Description = string.Empty
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

				//bundle.Issues.Add(itIssue); TODO: wtf its here?

				foreach (var vulnerabilityInfo in vulnerabilityGroup)
				{
					vulnerabilityInfo.IssueNumber = itIssue?.Id;
					vulnerabilityInfo.IssueUrl = itIssue?.Url;

					var additionalContent = string.Empty;

					if (!string.IsNullOrWhiteSpace(vulnerabilityInfo.Exploit))
						additionalContent += "\n" + vulnerabilityInfo.Exploit;

					if (!string.IsNullOrWhiteSpace(vulnerabilityInfo.AdditionalExploitConditions))
						additionalContent += "\n" + vulnerabilityInfo.AdditionalExploitConditions;

					var issue = new IssueAnnotation
					{
						LineStart = vulnerabilityInfo.NumberLine,
						File = vulnerabilityInfo.File,
						LongName = vulnerabilityInfo.Type,
						Severity = "High",
						Id = itIssue?.Id,
						IssuePath = itIssue?.Url,
						AdditionalContent = additionalContent
					};

					bundle.Changes.Add(new FileChangeAnnotation
					{
						Annotation = issue,
						Vulnerability = vulnerabilityInfo,
						Type = FileChangeAnnotationType.Add
					});
				}
			}
		}
	}
}