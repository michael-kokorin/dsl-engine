namespace Infrastructure.Reports.Translation
{
	using JetBrains.Annotations;

	using NReco.PdfGenerator;

	[UsedImplicitly]
	internal sealed class PdfReportTranslator : IReportTranslator
	{
		private const string HeaderFormat = "</br></br>";

		private const string FooterFormat = "<div><span class=\"page\"></span></br></br></br></div>";

		public byte[] Translate(string bodyHtml)
		{
			var pdfConverter = new HtmlToPdfConverter
			{
				LowQuality = false,
				PageHeaderHtml = HeaderFormat,
				PageFooterHtml = FooterFormat
			};

			return pdfConverter.GeneratePdf(bodyHtml);
		}
	}
}