namespace Workflow.GitHub.Stages.PostProcessing
{
	public interface IPostProcessingStage
	{
		IPostProcessingStage SetSuccessor(IPostProcessingStage postProcessingStage);

		void Execute(PostProcessingBundle bundle);
	}
}