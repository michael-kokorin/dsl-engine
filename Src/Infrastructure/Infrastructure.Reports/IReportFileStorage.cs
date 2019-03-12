namespace Infrastructure.Reports
{
	public interface IReportFileStorage
	{
		void SaveReportFile(ReportBundle reportBundle, ReportFile reportFile, long userId);
	}
}