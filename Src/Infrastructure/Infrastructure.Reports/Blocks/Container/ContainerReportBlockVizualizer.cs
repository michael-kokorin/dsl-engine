namespace Infrastructure.Reports.Blocks.Container
{
	using System;
	using System.Collections.Generic;
	using System.Web.UI;

	using JetBrains.Annotations;

	using Infrastructure.Reports.Generation.Stages;
	using Infrastructure.Reports.Html;

	[UsedImplicitly]
	internal sealed class ContainerReportBlockVizualizer : IReportBlockVizualizer<ContainerReportBlock>
	{
		private readonly IReportBlockVizualizationManager _reportBlockVizualizationManager;

		public ContainerReportBlockVizualizer([NotNull] IReportBlockVizualizationManager reportBlockVizualizationManager)
		{
			if (reportBlockVizualizationManager == null)
				throw new ArgumentNullException(nameof(reportBlockVizualizationManager));

			_reportBlockVizualizationManager = reportBlockVizualizationManager;
		}

		public void Vizualize(
			[NotNull] HtmlTextWriter htmlTextWriter,
			[NotNull] ContainerReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
		{
			if (htmlTextWriter == null) throw new ArgumentNullException(nameof(htmlTextWriter));
			if (block == null) throw new ArgumentNullException(nameof(block));

			var useTableRender = GetValue(parameterValues);

			if (useTableRender)
			{
				RenderTableContainer(htmlTextWriter, block, parameterValues, queryResults, userId);
			}
			else
			{
				RenderDvContainer(htmlTextWriter, block, parameterValues, queryResults, userId);
			}
		}

		private void RenderTableContainer(HtmlTextWriter htmlTextWriter,
			ContainerReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
		{
			var baseDivStyle = new HtmlStyle();

			baseDivStyle
				.Set(HtmlStyleKey.Width, "100%")
				.Set(HtmlStyleKey.Float, "none");

			if (block.BreakPageBefore)
				baseDivStyle.Set("page-break-before", "always");

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, baseDivStyle.ToString());
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, block.Id);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Table);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Td);

			var innerDivStyle = new HtmlStyle();

			var backgroundColor = block.BackgroundColor.ToRgb();

			innerDivStyle
				.Set(HtmlStyleKey.BackgroundColor, backgroundColor)
				.Set(HtmlStyleKey.Width, "100%")

				// unnessesary for the inner div block
				.Set(HtmlStyleKey.MarginBottom, $"{block.MarginBottomPx}px")
				.Set(HtmlStyleKey.MarginLeft, $"{block.MarginLeftPx}px")
				.Set(HtmlStyleKey.MarginRight, $"{block.MarginRightPx}px")
				.Set(HtmlStyleKey.MarginTop, $"{block.MarginTopPx}px")
				.Set(HtmlStyleKey.PaddingBottom, $"{block.PaddingBottomPx}px")
				.Set(HtmlStyleKey.PaddingLeft, $"{block.PaddingLeftPx}px")
				.Set(HtmlStyleKey.PaddingRight, $"{block.PaddingRightPx}px")
				.Set(HtmlStyleKey.PaddingTop, $"{block.PaddingTopPx}px");

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, innerDivStyle.ToString());
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, $"{block.Id}-Inner");
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Table);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);

			if (block.Orientation == ContainerOrientation.Horizontal)
				htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

			if (block.Childs != null)
			{
				var i = 0;

				foreach (var child in block.Childs)
				{
					var divWidth = 100;

					if (block.Orientation == ContainerOrientation.Horizontal)
					{
						if (block.ChildProportions == null)
							divWidth = 100 / block.Childs.Length;
						else
						{
							divWidth = block.ChildProportions[i];
						}
					}

					if (block.Orientation == ContainerOrientation.Vertical)
					{
						htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
					}

					var childDivStyle = new HtmlStyle();

					childDivStyle
						.Set(HtmlStyleKey.BackgroundColor, backgroundColor)
						.Set(HtmlStyleKey.Width, $"{divWidth}%");

					htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, childDivStyle.ToString());
					htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, $"{block.Id}-Child{i + 1}");

					htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Td);

					_reportBlockVizualizationManager.Vizualize(
						htmlTextWriter,
						(dynamic) child,
						parameterValues,
						queryResults,
						userId);

					htmlTextWriter.RenderEndTag(); // TD

					if (block.Orientation == ContainerOrientation.Vertical)
					{
						htmlTextWriter.RenderEndTag(); // TR
					}

					i++;
				}
			}

			if (block.Orientation == ContainerOrientation.Horizontal)
				htmlTextWriter.RenderEndTag(); // TR

			htmlTextWriter.RenderEndTag(); //outher table TBody
			htmlTextWriter.RenderEndTag(); //outher Table

			htmlTextWriter.RenderEndTag(); //outher table TD
			htmlTextWriter.RenderEndTag(); //outher table TR
			htmlTextWriter.RenderEndTag(); //outher table TBody
			htmlTextWriter.RenderEndTag(); //outher Table
		}

		private void RenderDvContainer(HtmlTextWriter htmlTextWriter,
			ContainerReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
		{
			var baseDivStyle = new HtmlStyle();

			baseDivStyle
				.Set(HtmlStyleKey.Display, block.Orientation == ContainerOrientation.Horizontal ? "table" : "block")
				.Set(HtmlStyleKey.Width, "100%")
				.Set(HtmlStyleKey.Float, "none");

			if (block.BreakPageBefore)
				baseDivStyle.Set("page-break-before", "always");

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, baseDivStyle.ToString());
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, block.Id);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Div);

			var innerDivStyle = new HtmlStyle();

			var backgroundColor = block.BackgroundColor.ToRgb();

			innerDivStyle
				.Set(HtmlStyleKey.BackgroundColor, backgroundColor)
				.Set(HtmlStyleKey.Width, "auto")
				.Set(HtmlStyleKey.Display, "block")
				.Set(HtmlStyleKey.Float, block.Orientation == ContainerOrientation.Horizontal ? "initial" : "none")

				// unnessesary for the inner div block
				.Set(HtmlStyleKey.MarginBottom, $"{block.MarginBottomPx}px")
				.Set(HtmlStyleKey.MarginLeft, $"{block.MarginLeftPx}px")
				.Set(HtmlStyleKey.MarginRight, $"{block.MarginRightPx}px")
				.Set(HtmlStyleKey.MarginTop, $"{block.MarginTopPx}px")
				.Set(HtmlStyleKey.PaddingBottom, $"{block.PaddingBottomPx}px")
				.Set(HtmlStyleKey.PaddingLeft, $"{block.PaddingLeftPx}px")
				.Set(HtmlStyleKey.PaddingRight, $"{block.PaddingRightPx}px")
				.Set(HtmlStyleKey.PaddingTop, $"{block.PaddingTopPx}px");

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, innerDivStyle.ToString());
			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, $"{block.Id}-Inner");
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Div);

			if (block.Childs != null)
			{
				var i = 0;

				foreach (var child in block.Childs)
				{
					var divWidth = 100;

					if (block.Orientation == ContainerOrientation.Horizontal)
					{
						if (block.ChildProportions == null)
							divWidth = 100 / block.Childs.Length;
						else
						{
							divWidth = block.ChildProportions[i];
						}
					}

					var childDivStyle = new HtmlStyle();

					childDivStyle
						.Set(HtmlStyleKey.BackgroundColor, backgroundColor)
						.Set(HtmlStyleKey.Display, block.Orientation == ContainerOrientation.Horizontal ? "table-cell" : "block")
						.Set(HtmlStyleKey.Width, $"{divWidth}%")
						.Set(HtmlStyleKey.Float, block.Orientation == ContainerOrientation.Horizontal ? "left" : "none");

					htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Style, childDivStyle.ToString());
					htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Id, $"{block.Id}-Child{i + 1}");
					htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Div); // child div

					_reportBlockVizualizationManager.Vizualize(
						htmlTextWriter,
						(dynamic) child,
						parameterValues,
						queryResults,
						userId);

					htmlTextWriter.RenderEndTag(); // child div

					i++;
				}

				if (block.Orientation == ContainerOrientation.Horizontal)
					htmlTextWriter.Write("<div style=\"clear: both;\"></div>");
			}

			htmlTextWriter.RenderEndTag(); // inner div
			htmlTextWriter.RenderEndTag(); //outher div
		}

		private static bool GetValue(IReadOnlyDictionary<string, object> parameterValues)
		{
			var useTable = false;

			// ReSharper disable once UseNullPropagationWhenPossible
			if (parameterValues == null)
				return false;

			if (!parameterValues.ContainsKey(DefaultReportParameters.ContainerUseTable)) return false;

			var value = parameterValues[DefaultReportParameters.ContainerUseTable];

			if (value is bool)
				useTable = (bool) value;
			else if (value is string)
			{
				useTable = bool.Parse((string) value);
			}

			return useTable;
		}
	}
}