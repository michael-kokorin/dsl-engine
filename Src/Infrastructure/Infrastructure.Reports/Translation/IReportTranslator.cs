namespace Infrastructure.Reports.Translation
{
	public interface IReportTranslator
	{
		byte[] Translate(string bodyHtml);
	}
}