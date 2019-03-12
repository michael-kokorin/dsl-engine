namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class LoadVulnInfoPostProcessingStage : PostProcessingStage
	{
		private readonly IVulnerabilityInfoProvider _vulnerabilityInfoProvider;

		public LoadVulnInfoPostProcessingStage([NotNull] IVulnerabilityInfoProvider vulnerabilityInfoProvider)
		{
			if (vulnerabilityInfoProvider == null) throw new ArgumentNullException(nameof(vulnerabilityInfoProvider));

			_vulnerabilityInfoProvider = vulnerabilityInfoProvider;
		}

		protected override Func<Tasks, bool> PreCondition => _ => true;

		protected override void ExecuteStage([NotNull] PostProcessingBundle bundle) =>
			bundle.VulnerabilitiesInfo = _vulnerabilityInfoProvider.GetByTask(bundle.Task);
	}
}