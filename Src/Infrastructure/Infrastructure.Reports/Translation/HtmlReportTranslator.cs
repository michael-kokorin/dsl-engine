namespace Infrastructure.Reports.Translation
{
	using System.Text;

	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class HtmlReportTranslator : IReportTranslator
	{
		public byte[] Translate(string bodyHtml) => Encoding.UTF8.GetBytes(bodyHtml);
	}
}