namespace Infrastructure.Reports.Translation
{
	using Common.Enums;

	public interface IReportTranslationManager
	{
		ReportFile Translate(ReportBundle reportBundle, ReportFileType reportFileType);
	}
}