namespace Infrastructure.Reports.Blocks.Label
{
	using System;
	using System.Collections.Generic;
	using System.Web.UI;

	using JetBrains.Annotations;

	using Common.Html;
	using Infrastructure.Reports.Html;
	using Infrastructure.Templates;

	[UsedImplicitly]
	internal sealed class LabelReportBlockVizualizer : IReportBlockVizualizer<LabelReportBlock>
	{
		private readonly IHtmlEncoder _htmlEncoder;

		private readonly ITemplateBuilder _templateBuilder;

		public LabelReportBlockVizualizer([NotNull] IHtmlEncoder htmlEncoder, [NotNull] ITemplateBuilder templateBuilder)
		{
			if (htmlEncoder == null) throw new ArgumentNullException(nameof(htmlEncoder));
			if (templateBuilder == null) throw new ArgumentNullException(nameof(templateBuilder));

			_htmlEncoder = htmlEncoder;
			_templateBuilder = templateBuilder;
		}

		public void Vizualize(
			[NotNull] HtmlTextWriter htmlTextWriter,
			[NotNull] LabelReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
		{
			if (htmlTextWriter == null) throw new ArgumentNullException(nameof(htmlTextWriter));
			if (block == null) throw new ArgumentNullException(nameof(block));

			var labelStyle = new HtmlStyle();

			labelStyle
				.Set(HtmlStyleKey.Display, "inline-block")
				.Set("word-break", "break-all")
				.Set("white-space", "pre-wrap")
				.Set("word-wrap", "break-word")
				.Set(HtmlStyleKey.Color, block.Color.ToRgb())
				.Set(HtmlStyleKey.FontFamily, block.FontStyle.FontFamily)
				.Set(HtmlStyleKey.FontStyle, block.FontStyle.Italic ? "italic" : "normal")
				.Set(HtmlStyleKey.FontSize, $"{block.FontStyle.FontSizePx}px")
				.Set(HtmlStyleKey.FontWeight, block.FontStyle.Bold ? "bold" : "normal")
				.Set(HtmlStyleKey.TextAlign, block.HorizontalAlign.ToString())
				.Set(HtmlStyleKey.VerticalAlign, block.VerticalAlign.ToString())
				.Set(HtmlStyleKey.Width, "100%");

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, labelStyle.ToString());
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, block.Id);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Label);

			if (!string.IsNullOrEmpty(block.Text))
			{
				var renderedTemplate = RenderTemplate(block, parameterValues);

				renderedTemplate = _htmlEncoder.Encode(renderedTemplate);

				htmlTextWriter.Write(renderedTemplate);
			}

			htmlTextWriter.RenderEndTag(); // label
		}

		private string RenderTemplate(LabelReportBlock block, IReadOnlyDictionary<string, object> parameterValues)
		{
			var template = _templateBuilder.Build(block.Text);

			if (parameterValues == null) return template.Render();

			foreach (var parameterValue in parameterValues)
			{
				template.Add(parameterValue.Key, parameterValue.Value);
			}

			try
			{
				return template.Render();
			}
			catch (Exception)
			{
				throw new Exception($"Incorrect label template. LabelId='{block.Id}'");
			}
		}
	}
}