namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Agent.Contracts;
	using Infrastructure.Plugins.Contracts;
	using Repository.Context;

	public sealed class PostProcessingBundle
	{
		public readonly List<FileChangeAnnotation> Changes = new List<FileChangeAnnotation>();

		public readonly Tasks Task;

		public IssueAnnotation[] IssueAnnotations = new IssueAnnotation[0];

		public IssueVulnerabilityLink[] IssueVulnerabilityLinks = new IssueVulnerabilityLink[0];

		public List<Issue> Issues = new List<Issue>();

		public IssueAnnotation[] RemainingIssues = new IssueAnnotation[0];

		public VulnerabilityInfo[] VulnerabilitiesInfo = new VulnerabilityInfo[0];

		public IVersionControlPlugin VersionControlPlugin;

		public IIssueTrackerPlugin IssueTrackerPlugin;

		public PostProcessingBundle([NotNull] Tasks task)
		{
			if(task == null)
			{
				throw new ArgumentNullException(nameof(task));
			}

			Task = task;
		}
	}
}