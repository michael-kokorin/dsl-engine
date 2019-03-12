namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class LoadIssuesPostProcessingStage : PostProcessingStage
	{
		protected override Func<Tasks, bool> PreCondition => task => task.Projects.CommitToIt;

		protected override void ExecuteStage(PostProcessingBundle bundle) =>
			bundle.Issues = bundle.IssueTrackerPlugin.GetIssues().ToList();
	}
}