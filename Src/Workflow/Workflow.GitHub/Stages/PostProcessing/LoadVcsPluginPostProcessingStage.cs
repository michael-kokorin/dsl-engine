namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Contracts;
	using Repository.Context;

	[UsedImplicitly]
	internal sealed class LoadVcsPluginPostProcessingStage : PostProcessingStage
	{
		private readonly IBackendPluginProvider _backendPluginProvider;

		public LoadVcsPluginPostProcessingStage([NotNull] IBackendPluginProvider backendPluginProvider)
		{
			if (backendPluginProvider == null) throw new ArgumentNullException(nameof(backendPluginProvider));

			_backendPluginProvider = backendPluginProvider;
		}

		protected override Func<Tasks, bool> PreCondition => task => task.Projects.CommitToVcs;

		protected override void ExecuteStage(PostProcessingBundle bundle)
		{
			if (bundle.Task.Projects.VcsPluginId == null)
				throw new Exception();

			bundle.VersionControlPlugin = _backendPluginProvider.GetPlugin<IVersionControlPlugin>(
				bundle.Task.Projects.VcsPluginId.Value,
				bundle.Task.CreatedById,
				bundle.Task.ProjectId);
		}
	}
}