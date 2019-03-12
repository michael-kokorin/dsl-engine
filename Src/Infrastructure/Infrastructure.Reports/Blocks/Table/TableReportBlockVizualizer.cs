namespace Infrastructure.Reports.Blocks.Table
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.UI;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Query.Result;
	using Infrastructure.Reports.Html;

	[UsedImplicitly]
	internal sealed class TableReportBlockVizualizer: IReportBlockVizualizer<TableReportBlock>
	{
		private readonly IReportBlockVizualizationManager _blockVizualizationManager;

		public TableReportBlockVizualizer([NotNull] IReportBlockVizualizationManager blockVizualizationManager)
		{
			if (blockVizualizationManager == null) throw new ArgumentNullException(nameof(blockVizualizationManager));

			_blockVizualizationManager = blockVizualizationManager;
		}

		public void Vizualize(
			[NotNull] HtmlTextWriter htmlTextWriter,
			[NotNull] TableReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			[NotNull] IReadOnlyCollection<ReportQueryResult> results,
			long userId)
		{
			if (htmlTextWriter == null) throw new ArgumentNullException(nameof(htmlTextWriter));
			if (block == null) throw new ArgumentNullException(nameof(block));
			if (results == null) throw new ArgumentNullException(nameof(results));

			if (string.IsNullOrEmpty(block.QueryKey))
				throw new IncorrectBlockQueryKeyException(block);

			var query = results
				.SingleOrDefault(r => r.Key.Equals(block.QueryKey, StringComparison.InvariantCultureIgnoreCase));

			if (query == null)
				throw new QueryResultNotFoundException(block);

			RenderTable(htmlTextWriter, block, query.Result, userId);
		}

		private void RenderBody(HtmlTextWriter htmlWriter, TableReportBlock block, QueryResult queryBlock, long userId)
		{
			var tableBodyStyle = new HtmlStyle();

			tableBodyStyle.Set(HtmlStyleKey.Border, $"{block.BorderPx}px solid black");

			htmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);

			foreach (var row in queryBlock.Items)
			{
				htmlWriter.AddAttribute(HtmlTextWriterAttribute.Style, tableBodyStyle.ToString());
				htmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

				foreach (var column in queryBlock.Columns)
				{
					htmlWriter.AddAttribute(HtmlTextWriterAttribute.Style, tableBodyStyle.ToString());
					htmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);

					var columnItem = row.Value.GetType()
						.GetProperty(column.Code)
						.GetValue(row.Value);

					if (columnItem != null)
					{
						_blockVizualizationManager.Vizualize(
							htmlWriter,
							(dynamic) block.BodyLabel,
							new ConcurrentDictionary<string, object>(
								new[]
								{
									new KeyValuePair<string, object>(TableReportBlockParameters.RowColumnItem, columnItem.ToString())
								}),
							null,
							userId);
					}

					htmlWriter.RenderEndTag(); // Td
				}

				htmlWriter.RenderEndTag(); // Tr
			}

			htmlWriter.RenderEndTag(); // Tbody
		}

		private void RenderHeader(HtmlTextWriter htmlWriter, TableReportBlock block, QueryResult queryBlock, long userId)
		{
			var tableHeadStyle = new HtmlStyle();

			tableHeadStyle.Set(HtmlStyleKey.Border, $"{block.BorderPx}px solid black");

			htmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
			htmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var column in queryBlock.Columns)
			{
				htmlWriter.AddAttribute(HtmlTextWriterAttribute.Style, tableHeadStyle.ToString());
				htmlWriter.RenderBeginTag(HtmlTextWriterTag.Th);

				_blockVizualizationManager.Vizualize(
					htmlWriter,
					(dynamic) block.HeaderLabel,
					new ConcurrentDictionary<string, object>(
						new[]
						{
							new KeyValuePair<string, object>(TableReportBlockParameters.ColumnHeader, column.Name)
						}),
					null,
					userId);

				htmlWriter.RenderEndTag(); // Th
			}

			htmlWriter.RenderEndTag(); // tr
			htmlWriter.RenderEndTag(); // thead
		}

		private void RenderTable(
			HtmlTextWriter htmlTextWriter,
			TableReportBlock block,
			QueryResult queryBlock,
			long userId)
		{
			var tableStyle = new HtmlStyle();

			tableStyle
				.Set(HtmlStyleKey.Width, "100%")
				.Set(HtmlStyleKey.Border, $"{block.BorderPx}px solid black")
				.Set(HtmlStyleKey.BorderCollapse, "collapse");

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, tableStyle.ToString());
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, block.Id);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Table);

			RenderHeader(htmlTextWriter, block, queryBlock, userId);

			RenderBody(htmlTextWriter, block, queryBlock, userId);

			htmlTextWriter.RenderEndTag(); // Table
		}
	}
}