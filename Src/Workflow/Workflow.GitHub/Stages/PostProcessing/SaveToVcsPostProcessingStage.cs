namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Logging;
	using Infrastructure.Telemetry;
	using Infrastructure.Telemetry.Entities;
	using Infrastructure.Templates;
	using Infrastructure.Vulnerability;
	using Repository.Context;
	using Workflow.GitHub.Extensions;
	using Workflow.VersionControl;

	[UsedImplicitly]
	internal sealed class SaveToVcsPostProcessingStage: PostProcessingStage
	{
		private readonly IBranchNameBuilder _branchNameBuilder;

		private readonly IIssueAnnotationFormatter _issueAnnotationFormatter;

		private readonly IIssueAnnotationSerializer _issueAnnotationSerializer;

		private readonly ILog _log;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		private readonly ITemplateProvider _templateProvider;

		private readonly IVulnerabilityShortTypeResolver _vulnerabilityShortTypeResolver;

		public SaveToVcsPostProcessingStage(
			[NotNull] IIssueAnnotationFormatter issueAnnotationFormatter,
			[NotNull] IIssueAnnotationSerializer issueAnnotationSerializer,
			[NotNull] IBranchNameBuilder branchNameBuilder,
			[NotNull] ILog log,
			[NotNull] IVulnerabilityShortTypeResolver vulnerabilityShortTypeResolver,
			[NotNull] ITemplateProvider templateProvider,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider)
		{
			if(issueAnnotationFormatter == null) throw new ArgumentNullException(nameof(issueAnnotationFormatter));
			if(issueAnnotationSerializer == null) throw new ArgumentNullException(nameof(issueAnnotationSerializer));
			if(branchNameBuilder == null) throw new ArgumentNullException(nameof(branchNameBuilder));
			if(log == null) throw new ArgumentNullException(nameof(log));
			if(vulnerabilityShortTypeResolver == null) throw new ArgumentNullException(nameof(vulnerabilityShortTypeResolver));
			if(templateProvider == null) throw new ArgumentNullException(nameof(templateProvider));
			if(telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));

			_issueAnnotationFormatter = issueAnnotationFormatter;
			_issueAnnotationSerializer = issueAnnotationSerializer;
			_branchNameBuilder = branchNameBuilder;
			_log = log;
			_vulnerabilityShortTypeResolver = vulnerabilityShortTypeResolver;
			_templateProvider = templateProvider;
			_telemetryScopeProvider = telemetryScopeProvider;
		}

		protected override Func<Tasks, bool> PreCondition => task => task.Projects.CommitToVcs;

		protected override void ExecuteStage(PostProcessingBundle bundle)
		{
			var branches = bundle.VersionControlPlugin.GetBranches().ToList();

			var mainBranch = branches.First(x => x.Name == bundle.Task.Repository);

			var annotationGroups = bundle.Changes
										.Where(x => x.Type != FileChangeAnnotationType.None)
										.GroupBy(x => x.Annotation.LongName);

			foreach(var typeGroup in annotationGroups)
			{
				var branchName = _branchNameBuilder.Build(new BranchNameBuilderInfo {Type = typeGroup.Key});

				var branch = branches.FirstOrDefault(
					x => string.Equals(
						x.Name,
						branchName,
						StringComparison.CurrentCultureIgnoreCase));

				if(branch == null)
				{
					using(
						var telemetryScope = _telemetryScopeProvider.Create<VcsPluginInfo>(TelemetryOperationNames.VcsPlugin.CreateBranch)
					)
					{
						try
						{
							telemetryScope.SetEntity(
								new VcsPluginInfo
								{
									Plugin = bundle.Task.Projects.Plugins1, // VCS plugin =)
									CreatedBranchName = branchName,
									TaskId = bundle.Task.Id
								});

							branch = bundle.VersionControlPlugin.CreateBranch(bundle.Task.FolderPath, branchName, mainBranch.Id);

							telemetryScope.WriteSuccess();
						}
						catch(Exception ex)
						{
							telemetryScope.WriteException(ex);

							throw;
						}
					}

					branches.Add(branch);
				}

				foreach(var fileGroup in typeGroup.GroupBy(x => x.Annotation.File))
				{
					var content = File.ReadAllText(Path.Combine(bundle.Task.FolderPath, fileGroup.Key));

					var ending = content.GetLineSplitter();

					var linesContent = content.Split(new[] {ending}, StringSplitOptions.None).ToList();

					var rowShift = 0;

					foreach(var fileUpdate in fileGroup.OrderBy(x => x.Annotation.LineStart))
					{
						var removeCount = 0;

						if(fileUpdate.Type == FileChangeAnnotationType.Update)
						{
							removeCount = fileUpdate.Annotation.LineEnd - fileUpdate.Annotation.LineStart + 1;

							for(var index = 0; index < removeCount; index++)
								linesContent.RemoveAt(fileUpdate.Annotation.LineStart + rowShift - 1);
						}

						var serializedIssueAnnotation = _issueAnnotationSerializer.Serialize(fileUpdate.Annotation);

						var annotation = _issueAnnotationFormatter.Format(serializedIssueAnnotation);

						var lines = annotation.Split('\n');

						var rawLine = linesContent[fileUpdate.Annotation.LineStart + rowShift - 1];
						var startLineShift = rawLine.Substring(0, rawLine.Length - rawLine.TrimStart(' ', '\t').Length);

						for(var index = 0; index < lines.Length; index++)
						{
							linesContent.Insert(
								fileUpdate.Annotation.LineStart + rowShift + index - 1,
								startLineShift + lines[index].Trim());
						}

						rowShift = rowShift - removeCount + lines.Length;
						if(fileUpdate.Vulnerability != null)
							fileUpdate.Vulnerability.NumberLine = fileUpdate.Annotation.LineStart + rowShift;
					}

					var memoryStream = new MemoryStream();
					var streamWriter = new StreamWriter(memoryStream);

					streamWriter.Write(string.Join(ending, linesContent));

					streamWriter.Flush();
					streamWriter.Close();

					var commitTemplate = _templateProvider.Get(TemplateNames.CommitName);

					commitTemplate.Body.Add(
									new Dictionary<string, object>
									{
										{"Group", _vulnerabilityShortTypeResolver.Resolve(typeGroup.Key)},
										{"File", Path.GetFileName(fileGroup.Key)}
									});

					var bodyCommit = commitTemplate.Body.Render();

					_log.Debug($"Commit to {branch.Id} {fileGroup.Key}");

					var fileContent = memoryStream.ToArray();

					using(var telemetryScope = _telemetryScopeProvider.Create<VcsPluginInfo>(TelemetryOperationNames.VcsPlugin.Commit))
					{
						try
						{
							telemetryScope.SetEntity(
								new VcsPluginInfo
								{
									Plugin = bundle.Task.Projects.Plugins1, // VCS plugin
									CommittedSourcesSize = fileContent.Length,
									TaskId = bundle.Task.Id
								});

							bundle.VersionControlPlugin.Commit(
									bundle.Task.FolderPath,
									branch.Id,
									bodyCommit,
									fileGroup.Key,
									fileContent);

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

			bundle.VersionControlPlugin.CleanUp(bundle.Task.FolderPath);
		}
	}
}