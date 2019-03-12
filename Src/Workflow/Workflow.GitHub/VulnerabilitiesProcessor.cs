namespace Workflow.GitHub
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;
	using Workflow.GitHub.Stages.PostProcessing;

	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
	internal sealed class VulnerabilitiesProcessor : IVulnerabilitiesProcessor
	{
		private readonly IPostProcessingStageProvider _postProcessingStageProvider;

		public VulnerabilitiesProcessor([NotNull] IPostProcessingStageProvider postProcessingStageProvider)
		{
			if (postProcessingStageProvider == null) throw new ArgumentNullException(nameof(postProcessingStageProvider));

			_postProcessingStageProvider = postProcessingStageProvider;
		}

		/// <summary>
		///   Processes the specified vulnerabilities.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <returns>
		///   Actual vulnerabilities.
		/// </returns>
		public TaskProcessingResult Process([NotNull] Tasks task)
		{
			if (task == null) throw new ArgumentNullException(nameof(task));

			var bundle = new PostProcessingBundle(task);

			var loadVulnInfoStage = _postProcessingStageProvider.Get<LoadVulnInfoPostProcessingStage>();
			var loadIssueAnnotationStage = _postProcessingStageProvider.Get<LoadIssueAnnotationsPostProcessingStage>();
			var filterByBranchStage = _postProcessingStageProvider.Get<FilterByBranchPostProcessingStage>();
			var updateVulnerStage = _postProcessingStageProvider.Get<UpdateVulnerabilitiesPostProcessingStage>();
			var updatePairsStage = _postProcessingStageProvider.Get<UpdatePairsPostProcessingStage>();
			var getRemainingIssuesStage = _postProcessingStageProvider.Get<GetRemainingIssuesPostProcessingStage>();
			var loadItPluginStage = _postProcessingStageProvider.Get<LoadItPluginPostProcessingStage>();
			var loadVcsPluginstage = _postProcessingStageProvider.Get<LoadVcsPluginPostProcessingStage>();
			var loadIssuesStage = _postProcessingStageProvider.Get<LoadIssuesPostProcessingStage>();
			var processRemainingIssues = _postProcessingStageProvider.Get<ProcessRemainingIssuesPostProcessingStage>();
			var saveToVcsStage = _postProcessingStageProvider.Get<SaveToVcsPostProcessingStage>();
			var saveToItStage = _postProcessingStageProvider.Get<SaveToItPostProcessingStage>();

			loadVulnInfoStage
				.SetSuccessor(loadIssueAnnotationStage)
				.SetSuccessor(filterByBranchStage)
				.SetSuccessor(updateVulnerStage)
				.SetSuccessor(updatePairsStage)
				.SetSuccessor(getRemainingIssuesStage)
				.SetSuccessor(loadItPluginStage)
				.SetSuccessor(loadVcsPluginstage)
				.SetSuccessor(loadIssuesStage)
				.SetSuccessor(processRemainingIssues)
				.SetSuccessor(saveToVcsStage)
				.SetSuccessor(saveToItStage);

			loadVulnInfoStage.Execute(bundle);

			return new TaskProcessingResult
			{
				FalsePositivePairs =
					bundle.IssueVulnerabilityLinks.Where(x => x.IssueAnnotation.State == IssueAnnotationState.FalsePositive).ToArray(),
				FalsePositiveAnnotations =
					bundle.RemainingIssues.Where(x => x.State == IssueAnnotationState.FalsePositive).ToArray(),
				Fixed = bundle.RemainingIssues
					.Concat(bundle.Changes.Where(x => x.Type == FileChangeAnnotationType.Update).Select(x => x.Annotation))
					.Where(x => x.State == IssueAnnotationState.Fixed)
					.ToArray(),
				Todo =
					bundle.Changes.Where(x => x.Annotation.State == IssueAnnotationState.Todo)
						.Select(x => new IssueVulnerabilityLink(x.Annotation, x.Vulnerability))
						.ToArray(),
				Reopen =
					bundle.Changes.Where(x => x.Annotation.State == IssueAnnotationState.Reopen)
						.Select(x => new IssueVulnerabilityLink(x.Annotation, x.Vulnerability))
						.ToArray()
			};
		}
	}
}