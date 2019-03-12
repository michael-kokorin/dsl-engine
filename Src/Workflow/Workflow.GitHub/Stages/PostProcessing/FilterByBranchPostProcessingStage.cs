namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;
	using Workflow.VersionControl;

	[UsedImplicitly]
	internal sealed class FilterByBranchPostProcessingStage : PostProcessingStage
	{
		private readonly IBranchNameBuilder _branchNameBuilder;

		public FilterByBranchPostProcessingStage([NotNull] IBranchNameBuilder branchNameBuilder)
		{
			if (branchNameBuilder == null) throw new ArgumentNullException(nameof(branchNameBuilder));

			_branchNameBuilder = branchNameBuilder;
		}

		protected override Func<Tasks, bool> PreCondition => _ => true;

		protected override void ExecuteStage(PostProcessingBundle bundle)
		{
			if (!_branchNameBuilder.IsOurBranch(bundle.Task.Repository)) return;

			var branchVulnerabilityTypeName = _branchNameBuilder.GetInfo(bundle.Task.Repository);

			bundle.VulnerabilitiesInfo = bundle.VulnerabilitiesInfo
				.Where(x => x.Type.ToLower() == branchVulnerabilityTypeName.Type)
				.ToArray();

			bundle.IssueAnnotations = bundle.IssueAnnotations
				.Where(x => x.LongName.ToLower() == branchVulnerabilityTypeName.Type)
				.ToArray();
		}
	}
}