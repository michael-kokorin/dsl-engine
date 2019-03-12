namespace Infrastructure.Reports.Generation.Stages
{
	public interface IReportGenerationStageDispatcher
	{
		IReportGenerationStage Get<T>() where T : IReportGenerationStage;
	}
}