namespace Infrastructure.Reports.Blocks.Html
{
	using System;
	using System.Collections.Generic;
	using System.Web.UI;

	using JetBrains.Annotations;

	using Infrastructure.Reports.Html;
	using Infrastructure.Templates;

	using ITemplate = Infrastructure.Templates.ITemplate;

	[UsedImplicitly]
	internal sealed class HtmlReportBlockVizualizer : IReportBlockVizualizer<HtmlReportBlock>
	{
		private readonly ITemplateBuilder _templateBuilder;

		public HtmlReportBlockVizualizer([NotNull] ITemplateBuilder templateBuilder)
		{
			if (templateBuilder == null) throw new ArgumentNullException(nameof(templateBuilder));

			_templateBuilder = templateBuilder;
		}

		public void Vizualize([NotNull] HtmlTextWriter htmlTextWriter,
			[NotNull] HtmlReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
		{
			if (htmlTextWriter == null) throw new ArgumentNullException(nameof(htmlTextWriter));
			if (block == null) throw new ArgumentNullException(nameof(block));

			if (string.IsNullOrEmpty(block.Template))
				return;

			var iteratorStyle = new HtmlStyle();

			iteratorStyle.Set(HtmlStyleKey.Width, "100%");

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, iteratorStyle.ToString());
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, block.Id);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Div);

			var renderedTemplate = GetTemplate(block, parameterValues, queryResults);

			htmlTextWriter.Write(renderedTemplate);

			htmlTextWriter.RenderEndTag(); // div
		}

		private string GetTemplate(HtmlReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults)
		{
			var template = _templateBuilder.Build(block.Template);

			RenderParameters(parameterValues, template);

			RenderQueryResults(queryResults, template);

			return template.Render();
		}

		private static void RenderParameters(IReadOnlyDictionary<string, object> parameterValues, ITemplate template)
		{
			if (parameterValues == null) return;

			foreach (var parameter in parameterValues)
			{
				template.Add(parameter.Key, parameter.Value);
			}
		}

		private static void RenderQueryResults(IReadOnlyCollection<ReportQueryResult> queryResults, ITemplate template)
		{
			if (queryResults == null) return;

			foreach (var queryResult in queryResults)
			{
				template.Add(queryResult.Key, queryResult.Result.Items);
			}
		}
	}
}