namespace Infrastructure.Reports.Blocks.Image
{
	using System;
	using System.Collections.Generic;
	using System.Web.UI;

	using JetBrains.Annotations;

	using Common.Html;
	using Infrastructure.Reports.Html;

	[UsedImplicitly]
	internal sealed class ImageReportBlockVizualizer : IReportBlockVizualizer<ImageReportBlock>
	{
		private readonly IHtmlEncoder _htmlEncoder;

		public ImageReportBlockVizualizer([NotNull] IHtmlEncoder htmlEncoder)
		{
			if (htmlEncoder == null) throw new ArgumentNullException(nameof(htmlEncoder));

			_htmlEncoder = htmlEncoder;
		}

		public void Vizualize([NotNull] HtmlTextWriter htmlTextWriter,
			[NotNull] ImageReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
		{
			if (htmlTextWriter == null) throw new ArgumentNullException(nameof(htmlTextWriter));
			if (block == null) throw new ArgumentNullException(nameof(block));

			var imgStyle = new HtmlStyle();

			imgStyle
				.Set(HtmlStyleKey.Width, _htmlEncoder.Encode(block.Width))
				.Set(HtmlStyleKey.Height, _htmlEncoder.Encode(block.Height));

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Alt, block.Alt);
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Src, block.Source);
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, imgStyle.ToString());
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, block.Id);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Img);
			htmlTextWriter.RenderEndTag();
		}
	}
}