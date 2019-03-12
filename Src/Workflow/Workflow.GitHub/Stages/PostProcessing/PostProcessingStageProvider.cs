namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	[UsedImplicitly]
	internal sealed class PostProcessingStageProvider : IPostProcessingStageProvider
	{
		private readonly IUnityContainer _container;

		public PostProcessingStageProvider([NotNull] IUnityContainer container)
		{
			if (container == null) throw new ArgumentNullException(nameof(container));

			_container = container;
		}

		public TStage Get<TStage>() where TStage : IPostProcessingStage => _container.Resolve<TStage>();
	}
}