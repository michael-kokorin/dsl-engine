namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class GetRemainingIssuesPostProcessingStage : PostProcessingStage
	{
		private readonly IIssueAnnotationWorkflow _issueAnnotationWorkflow;

		public GetRemainingIssuesPostProcessingStage([NotNull] IIssueAnnotationWorkflow issueAnnotationWorkflow)
		{
			if (issueAnnotationWorkflow == null) throw new ArgumentNullException(nameof(issueAnnotationWorkflow));

			_issueAnnotationWorkflow = issueAnnotationWorkflow;
		}

		protected override Func<Tasks, bool> PreCondition => _ => true;

		protected override void ExecuteStage(PostProcessingBundle bundle) =>
			bundle.RemainingIssues = ProcessRemainingIssues(bundle);

		private IssueAnnotation[] ProcessRemainingIssues(PostProcessingBundle bundle)
		{
			var remainingIssues = bundle.IssueAnnotations
				.Where(x => bundle.IssueVulnerabilityLinks.All(y => y.IssueAnnotation != x)).ToList();

			// fixed
			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var issueAnnotation in remainingIssues.ToArray())
			{
				if ((issueAnnotation.State == IssueAnnotationState.Fixed) ||
					(issueAnnotation.State == IssueAnnotationState.FalsePositive))
					continue;

				remainingIssues.Remove(issueAnnotation);

				_issueAnnotationWorkflow.ChangeState(issueAnnotation, IssueAnnotationState.Fixed);

				bundle.Changes.Add(new FileChangeAnnotation
				{
					Annotation = issueAnnotation,
					Type = FileChangeAnnotationType.Update
				});
			}

			return remainingIssues.ToArray();
		}
	}
}