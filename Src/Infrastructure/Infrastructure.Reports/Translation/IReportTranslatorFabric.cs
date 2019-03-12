namespace Infrastructure.Reports.Translation
{
	using Common.Enums;

	public interface IReportTranslatorFabric
	{
		IReportTranslator GetTranslator(ReportFileType reportFileType);
	}
}
