namespace Infrastructure.Reports.Blocks.Chart
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web.UI;

	using JetBrains.Annotations;

	using Infrastructure.Reports.Html;

	[UsedImplicitly]
	internal sealed class ChartReportBlockVizualizer : IReportBlockVizualizer<ChartReportBlock>
	{
		public void Vizualize(
			[NotNull] HtmlTextWriter htmlTextWriter,
			[NotNull] ChartReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			[NotNull] IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
		{
			if (htmlTextWriter == null) throw new ArgumentNullException(nameof(htmlTextWriter));
			if (block == null) throw new ArgumentNullException(nameof(block));
			if (queryResults == null) throw new ArgumentNullException(nameof(queryResults));

			var query =
				queryResults.SingleOrDefault(_ => _.Key.Equals(block.QueryKey, StringComparison.InvariantCultureIgnoreCase));

			if (query == null)
				throw new QueryResultNotFoundException(block);

			var chartId = Guid.NewGuid().ToString("N");

			var chartDivStyle = new HtmlStyle();

			chartDivStyle
				.Set(HtmlStyleKey.Width, "100%")
				.Set(HtmlStyleKey.Float, "left");

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, chartDivStyle.ToString());
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, block.Id);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Div);

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, chartId);
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Height, block.HeightPx.ToString());
			htmlTextWriter.RenderBeginTag("canvas");
			htmlTextWriter.RenderEndTag(); // canvas

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Script);

			var script = GenerateScript(block, chartId, query);

			htmlTextWriter.Write(script);

			htmlTextWriter.RenderEndTag(); // Script
			htmlTextWriter.RenderEndTag(); // div
		}

		private static StringBuilder GenerateScript(ChartReportBlock block, string chartId, ReportQueryResult query)
		{
			var sb = new StringBuilder();

			sb.AppendLine($"var ctx = document.getElementById(\"{chartId}\").getContext(\"2d\");;");

			sb.AppendLine($"ctx.canvas.height = {block.HeightPx};");

			var chartType = GetChartType(block);

			sb.AppendLine($"var myChart = new Chart(ctx, {{type: '{chartType}', data: {{ labels: [");

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var labelItem in query.Result.Items)
			{
				var property = labelItem.Value.GetType().GetProperty(block.Label.ColumnKey);

				if (property == null)
					throw new Exception($"Label property is not defined. Property='{block.Label.ColumnKey}'");

				var labelValue = property.GetValue(labelItem.Value);

				sb.Append($"\"{labelValue}\",");
			}

			sb.AppendLine("], datasets: [");

			RenderGraph(block, query, sb);

			sb.AppendLine("]},");

			sb.AppendLine("options: { maintainAspectRatio: false, scales: { yAxes: [{ticks: {beginAtZero:true}}]}}});");

			return sb;
		}

		private static void RenderGraph(ChartReportBlock block, ReportQueryResult query, StringBuilder sb)
		{
			switch (block.Type)
			{
				case ChartType.Line:
				case ChartType.Bar:
					RenderLineOrBar(block, sb, query);
					break;
				case ChartType.Doughnut:
				case ChartType.Pie:
					RenderPie(block, sb, query);
					break;
				default:
					throw new Exception($"Block type is not supported. Type='{block.Type}'");
			}
		}

		private static string GetChartType(ChartReportBlock block)
		{
			string chartType;
			switch (block.Type)
			{
				case ChartType.Bar:
					chartType = "bar";
					break;
				case ChartType.Line:
					chartType = "line";
					break;
				case ChartType.Pie:
					chartType = "pie";
					break;
				case ChartType.Doughnut:
					chartType = "doughnut";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return chartType;
		}

		private static void RenderLineOrBar(ChartReportBlock block, StringBuilder sb, ReportQueryResult query)
		{
			var labels = new List<string>();

			foreach (var blockColumn in block.Columns)
			{
				var labelSb = new StringBuilder();

				var fill = blockColumn.Fill ? "true" : "false";

				labelSb.AppendLine($"{{ label: '{blockColumn.DisplayName}', fill: {fill},");

				labelSb.AppendLine($"borderWidth: {blockColumn.BorderWidth},");

				var backgroundColor = blockColumn.BackgroundColor.ToRgba();
				var lineColor = blockColumn.LineColor.ToRgba();

				// just fixed colors
				labelSb.AppendLine($"backgroundColor: \"{backgroundColor}\",");
				labelSb.AppendLine($"borderColor: \"{lineColor}\",");

				labelSb.AppendLine($"hoverBackgroundColor: \"{backgroundColor}\",");
				labelSb.AppendLine($"hoverBorderColor: \"{lineColor}\",");

				labelSb.Append("data: [");

				// ReSharper disable once LoopCanBePartlyConvertedToQuery
				foreach (var columnItem in query.Result.Items)
				{
					var property = columnItem.Value.GetType().GetProperty(blockColumn.ColumnKey);

					if (property == null)
						throw new Exception($"Property is not defined. Property='{blockColumn.ColumnKey}'");

					var columnValue = property.GetValue(columnItem.Value);

					labelSb.Append($"{columnValue},");
				}

				labelSb.AppendLine("]");

				labelSb.AppendLine("}");

				labels.Add(labelSb.ToString());
			}

			var labelString = string.Join(",", labels);

			sb.AppendLine(labelString);
		}

		private static void RenderPie(ChartReportBlock block, StringBuilder sb, ReportQueryResult query)
		{
			sb.Append("{data: [");

			foreach (var blockColumn in block.Columns)
			{
				// ReSharper disable once LoopCanBePartlyConvertedToQuery
				foreach (var columnItem in query.Result.Items)
				{
					var property = columnItem.Value.GetType().GetProperty(blockColumn.ColumnKey);

					if (property == null)
						throw new Exception($"Property is not defined. Property='{blockColumn.ColumnKey}'");

					var columnValue = property.GetValue(columnItem.Value);

					sb.Append($"{columnValue},");
				}
			}

			sb.AppendLine("],");

			sb.Append("backgroundColor: [");

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var columns in block.Columns)
			{
				foreach (var columnItem in query.Result.Items)
				{
					sb.Append($"\"{new ReportItemColor(1.0f).ToHex()}\",");
				}
			}

			sb.AppendLine("],");

			sb.Append("hoverBackgroundColor: [");

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var columns in block.Columns)
			{
				foreach (var columnItem in query.Result.Items)
				{
					sb.Append($"\"{new ReportItemColor(1.0f).ToHex()}\",");
				}
			}

			sb.AppendLine("]}");
		}
	}
}