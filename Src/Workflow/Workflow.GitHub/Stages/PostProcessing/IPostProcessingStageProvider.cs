namespace Workflow.GitHub.Stages.PostProcessing
{
	public interface IPostProcessingStageProvider
	{
		TStage Get<TStage>() where TStage : IPostProcessingStage;
	}
}