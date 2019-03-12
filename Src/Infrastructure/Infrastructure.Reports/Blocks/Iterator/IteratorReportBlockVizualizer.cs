namespace Infrastructure.Reports.Blocks.Iterator
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.UI;

	using JetBrains.Annotations;

	using Infrastructure.Reports.Html;

	[UsedImplicitly]
	public sealed class IteratorReportBlockVizualizer : IReportBlockVizualizer<IteratorReportBlock>
	{
		private readonly IReportBlockVizualizationManager _reportBlockVizualizationManager;

		public IteratorReportBlockVizualizer([NotNull] IReportBlockVizualizationManager reportBlockVizualizationManager)
		{
			if (reportBlockVizualizationManager == null)
				throw new ArgumentNullException(nameof(reportBlockVizualizationManager));

			_reportBlockVizualizationManager = reportBlockVizualizationManager;
		}

		public void Vizualize(
			[NotNull] HtmlTextWriter htmlTextWriter,
			[NotNull] IteratorReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
		{
			if (htmlTextWriter == null) throw new ArgumentNullException(nameof(htmlTextWriter));
			if (block == null) throw new ArgumentNullException(nameof(block));

			if (string.IsNullOrEmpty(block.QueryKey))
				throw new IncorrectBlockQueryKeyException(block);

			var query = queryResults
				.SingleOrDefault(_ => _.Key.Equals(block.QueryKey, StringComparison.InvariantCultureIgnoreCase));

			if (query == null)
				throw new QueryResultNotFoundException(block);

			if (block.Child == null)
				return;

			var iteratorStyle = new HtmlStyle();

			iteratorStyle.Set(HtmlStyleKey.Width, "100%");
			iteratorStyle.Set(HtmlStyleKey.Float, "none");

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, iteratorStyle.ToString());
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, block.Id);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Div);

			var i = 1;

			var queryItemKey = $"{query.Key}Item";

			foreach (var resultItem in query.Result.Items)
			{
				htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, iteratorStyle.ToString());
				htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, $"{block.Id}-Item{i}");
				htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Div);

				var childParameters = parameterValues.ToDictionary(
					paramValue => paramValue.Key,
					paramValue => paramValue.Value);

				childParameters.Add(queryItemKey, resultItem.Value);

				_reportBlockVizualizationManager.Vizualize(
					htmlTextWriter,
					(dynamic) block.Child,
					childParameters,
					queryResults,
					userId);

				htmlTextWriter.RenderEndTag(); // div

				i++;
			}

			htmlTextWriter.RenderEndTag(); // div
		}
	}
}