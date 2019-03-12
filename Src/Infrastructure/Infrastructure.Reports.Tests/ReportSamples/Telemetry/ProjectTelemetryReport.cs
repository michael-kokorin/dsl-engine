namespace Infrastructure.Reports.Tests.ReportSamples.Telemetry
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Reports.Blocks;
	using Infrastructure.Reports.Blocks.Chart;
	using Infrastructure.Reports.Blocks.Container;
	using Infrastructure.Reports.Blocks.HtmlDoc;
	using Infrastructure.Reports.Blocks.Label;

	[TestFixture]
	public sealed class ProjectTelemetryReport
	{
		[Test]
		public void ShouldGenerateProjectTelemetryReport()
		{
			const string queryKey = "ProjectOperations";

			var reportRule = new ReportRule
			{
				Parameters = new ReportParameter[0],
				QueryLinks =
					new IReportQuery[]
					{
						new ReportQuery
						{
							Key = queryKey,
							Text = @"ProjectTelemetry
order #DateTimeLocal# asc
select
DateTimeLocal=#DateTimeLocal.ToString()#
#OperationName#
#OperationDuration#
#UserLogin#
#OperationStatus#
#ProjectName#
select end"
						}
					},
				ReportTitle = "Project telemetry",
				Template = new ReportTemplate
				{
					Root = new HtmlDocReportBlock(true)
					{
						Id = "HtmlRootBlock",
						Child = new ContainerReportBlock("MainContainerBlock")
						{
							Orientation = ContainerOrientation.Vertical,
							Childs = new[]
							{
								VcsTelemetryReport.GetHeader(),
								GetOperationDurationChart(queryKey),
								VcsTelemetryReport.GetTable(
									queryKey,
									"Table 1. Operations list")
							}
						}
					}
				}
			};

			var serializedRule = reportRule.ToJson();

			serializedRule.Should().NotBeNullOrEmpty();
		}

		public static IReportBlock GetOperationDurationChart(string query)
		{
			var chartContainer = new ContainerReportBlock("OperationDurationChartContainerBlock")
			{
				Orientation = ContainerOrientation.Vertical,
				Childs = new IReportBlock[]
				{
					new ChartReportBlock
					{
						Columns = new[]
						{
							new ChartColumn
							{
								ColumnKey = "OperationDuration",
								DisplayName = "Operation duration [ms]"
							}
						},
						HeightPx = 250,
						Label = new ChartLabel
						{
							ColumnKey = "DateTimeLocal"
						},
						Type = ChartType.Line,
						QueryKey = query
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 14,
							Bold = true
						},
						Text = "Chart A. Operation duration",
						HorizontalAlign = LabelHorizontalAlign.Center
					}
				}
			};

			return chartContainer;
		}
	}
}