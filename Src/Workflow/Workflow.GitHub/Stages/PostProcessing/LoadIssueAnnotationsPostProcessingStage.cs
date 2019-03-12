namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class LoadIssueAnnotationsPostProcessingStage : PostProcessingStage
	{
		private readonly IAnnotationIssuesProvider _annotationIssuesProvider;

		public LoadIssueAnnotationsPostProcessingStage([NotNull] IAnnotationIssuesProvider annotationIssuesProvider)
		{
			if (annotationIssuesProvider == null) throw new ArgumentNullException(nameof(annotationIssuesProvider));

			_annotationIssuesProvider = annotationIssuesProvider;
		}

		protected override Func<Tasks, bool> PreCondition => _ => true;

		protected override void ExecuteStage(PostProcessingBundle bundle)
			=> bundle.IssueAnnotations = GetIssuesByTask(bundle.Task);

		private IssueAnnotation[] GetIssuesByTask(Tasks task) =>
			_annotationIssuesProvider
				.GetIssues(task.FolderPath, task.AnalyzedFiles.Split('\n'))
				.GroupBy(x => new
				{
					x.File,
					x.LineStart
				})
				.Select(x => x.First())
				.ToArray();
	}
}