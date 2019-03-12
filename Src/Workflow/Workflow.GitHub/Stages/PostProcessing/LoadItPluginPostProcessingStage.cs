namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Contracts;
	using Repository.Context;

	[UsedImplicitly]
	public sealed class LoadItPluginPostProcessingStage : PostProcessingStage
	{
		private readonly IBackendPluginProvider _backendPluginProvider;

		public LoadItPluginPostProcessingStage([NotNull] IBackendPluginProvider backendPluginProvider)
		{
			if (backendPluginProvider == null) throw new ArgumentNullException(nameof(backendPluginProvider));

			_backendPluginProvider = backendPluginProvider;
		}

		protected override Func<Tasks, bool> PreCondition => task => task.Projects.CommitToIt;

		protected override void ExecuteStage(PostProcessingBundle bundle)
		{
			if (bundle.Task.Projects.ItPluginId == null)
				throw new Exception("Project IT plugin is not initialized.");

			bundle.IssueTrackerPlugin = _backendPluginProvider.GetPlugin<IIssueTrackerPlugin>(
				bundle.Task.Projects.ItPluginId.Value,
				bundle.Task.CreatedById,
				bundle.Task.ProjectId);
		}
	}
}