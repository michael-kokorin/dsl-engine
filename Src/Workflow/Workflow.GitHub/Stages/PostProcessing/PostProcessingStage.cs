namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;

	using JetBrains.Annotations;

	using Repository.Context;

	public abstract class PostProcessingStage : IPostProcessingStage
	{
		private IPostProcessingStage _successor;

		protected abstract Func<Tasks, bool> PreCondition { get; }

		public IPostProcessingStage SetSuccessor([NotNull] IPostProcessingStage postProcessingStage)
		{
			if (postProcessingStage == null) throw new ArgumentNullException(nameof(postProcessingStage));

			_successor = postProcessingStage;

			return _successor;
		}

		public void Execute([NotNull] PostProcessingBundle bundle)
		{
			if (bundle == null) throw new ArgumentNullException(nameof(bundle));

			if (PreCondition.Invoke(bundle.Task))
				ExecuteStage(bundle);

			ExecuteSuccessor(bundle);
		}

		protected abstract void ExecuteStage(PostProcessingBundle bundle);

		private void ExecuteSuccessor(PostProcessingBundle bundle) => _successor?.Execute(bundle);
	}
}