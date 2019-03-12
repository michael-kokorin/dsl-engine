namespace Infrastructure.Reports.Blocks.HtmlDoc
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Web.UI;

	using JetBrains.Annotations;

	using Infrastructure.Reports.Blocks.Chart;
	using Infrastructure.Reports.Html;

	[UsedImplicitly]
	internal sealed class HtmlDocReportBlockVizualizer : IReportBlockVizualizer<HtmlDocReportBlock>
	{
		private readonly IChartScriptProvider _chartScriptProvider;

		private readonly IReportBlockVizualizationManager _reportBlockVizualizationManager;

		public HtmlDocReportBlockVizualizer([NotNull] IChartScriptProvider chartScriptProvider,
			[NotNull] IReportBlockVizualizationManager reportBlockVizualizationManager)
		{
			if (chartScriptProvider == null) throw new ArgumentNullException(nameof(chartScriptProvider));
			if (reportBlockVizualizationManager == null)
				throw new ArgumentNullException(nameof(reportBlockVizualizationManager));

			_chartScriptProvider = chartScriptProvider;
			_reportBlockVizualizationManager = reportBlockVizualizationManager;
		}

		public void Vizualize(
			[NotNull] HtmlTextWriter htmlTextWriter,
			[NotNull] HtmlDocReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
		{
			if (htmlTextWriter == null) throw new ArgumentNullException(nameof(htmlTextWriter));
			if (block == null) throw new ArgumentNullException(nameof(block));

			var docType = new StringBuilder(block.WithHeader ? "<!DOCTYPE html>" : string.Empty);

			htmlTextWriter.Write(docType);

			if (block.WithHeader)
			{
				htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Html);
				htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Head);
				htmlTextWriter.AddAttribute("charset", "UTF-8");
				htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Meta);
				htmlTextWriter.RenderEndTag(); // meta
			}

			RenderChartScript(htmlTextWriter);

			if (block.WithHeader)
			{
				htmlTextWriter.RenderEndTag(); // head
				htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Body);
			}

			var containerStyle = new HtmlStyle();

			containerStyle
				.Set(HtmlStyleKey.Width, block.Width)
				.Set(HtmlStyleKey.AlignContent, "center")
				.Set(HtmlStyleKey.Margin, "0 auto");

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, containerStyle.ToString());
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, block.Id);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Div); // div

			_reportBlockVizualizationManager.Vizualize(
				htmlTextWriter,
				(dynamic) block.Child,
				parameterValues,
				queryResults,
				userId);

			htmlTextWriter.RenderEndTag(); // div

			// ReSharper disable once InvertIf
			if (block.WithHeader)
			{
				htmlTextWriter.RenderEndTag(); // body
				htmlTextWriter.RenderEndTag(); // html
			}
		}

		private void RenderChartScript(HtmlTextWriter htmlWriter)
		{
			var chartScript = _chartScriptProvider.GetScript();

			htmlWriter.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			htmlWriter.AddAttribute("language", "JavaScript");
			htmlWriter.RenderBeginTag(HtmlTextWriterTag.Script);
			htmlWriter.Write(chartScript);
			htmlWriter.RenderEndTag(); // script
		}
	}
}