namespace Infrastructure.Reports.Generation.Stages
{
	public interface IReportGenerationStage
	{
		void SetSuccessor(IReportGenerationStage reportGenerationStage);

		void Process(ReportBundle reportBundle);
	}
}