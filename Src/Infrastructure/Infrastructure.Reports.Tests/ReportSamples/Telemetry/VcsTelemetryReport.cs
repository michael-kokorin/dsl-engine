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
	using Infrastructure.Reports.Blocks.Table;

	[TestFixture]
	public sealed class VcsTelemetryReport
	{
		public static IReportBlock GetHeader()
		{
			var headerBlock = new ContainerReportBlock("HeaderContainerBlock")
			{
				Orientation = ContainerOrientation.Horizontal,
				MarginBottomPx = 20,
				Childs = new IReportBlock[]
				{
					new ContainerReportBlock("HeaderLeftContainerBlock")
					{
						Orientation = ContainerOrientation.Vertical,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 24
								},
								Id = "LabelReportHeaderCompany",
								Text = "Positive Technologies AI SSDL"
							},
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 24
								},
								Id = "LabelReportHeaderReportName",
								Text = "Report: $ReportTitle$"
							}
						}
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 24
						},
						Id = "LabelReportCreationTime",
						Text = "$CurrentDate$",
						HorizontalAlign = LabelHorizontalAlign.Right,
						VerticalAlign = LabelVerticalalign.Middle
					}
				}
			};

			return headerBlock;
		}

		private static IReportBlock GetDohnuts()
		{
			var block = new ContainerReportBlock("DoughnutsContainer")
			{
				Orientation = ContainerOrientation.Horizontal,
				Childs = new IReportBlock[]
				{
					new ContainerReportBlock
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
										BorderWidth = 0,
										ColumnKey = "Count",
										DisplayName = "Count"
									}
								},
								HeightPx = 250,
								Label = new ChartLabel
								{
									ColumnKey = "Operation"
								},
								Type = ChartType.Doughnut,
								QueryKey = "VcsOperationsByName"
							},
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 14,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Chart A. Operations by key",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					},
					new ContainerReportBlock
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
										BorderWidth = 0,
										ColumnKey = "Count",
										DisplayName = "Count"
									}
								},
								HeightPx = 250,
								Label = new ChartLabel
								{
									ColumnKey = "Type"
								},
								Type = ChartType.Doughnut,
								QueryKey = "VcsOperationsByType"
							},
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 14,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Chart B. Operations by plugin type",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					}
				}
			};

			return block;
		}

		private static IReportBlock GetCharts()
		{
			var block = new ContainerReportBlock("ChartsPart")
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
								DisplayName = "Operation duration [s]"
							}
						},
						HeightPx = 200,
						Label = new ChartLabel
						{
							ColumnKey = "DateTimeLocal"
						},
						Type = ChartType.Line,
						QueryKey = "VcsOperations"
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 14,
							Bold = true
						},
						Text = "Chart C. All VCS operations duration",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ChartReportBlock
					{
						Columns = new[]
						{
							new ChartColumn
							{
								ColumnKey = "OperationDuration",
								DisplayName = "operation duration [s]"
							}
						},
						HeightPx = 200,
						Label = new ChartLabel
						{
							ColumnKey = "Time"
						},
						Type = ChartType.Line,
						QueryKey = "VcsCheckoutOperations"
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 14,
							Bold = true
						},
						Text = "Chart D. Chekout VCS operations duration",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ChartReportBlock
					{
						Columns = new[]
						{
							new ChartColumn
							{
								ColumnKey = "OperationDuration",
								DisplayName = "Operation duration [s]"
							}
						},
						HeightPx = 200,
						Label = new ChartLabel
						{
							ColumnKey = "Time"
						},
						Type = ChartType.Line,
						QueryKey = "VcsCreateBranchOperations"
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 14,
							Bold = true
						},
						Text = "Chart E. Create Branch VCS operations duration",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ChartReportBlock
					{
						Columns = new[]
						{
							new ChartColumn
							{
								ColumnKey = "OperationDuration",
								DisplayName = "Operation duration [s]"
							}
						},
						HeightPx = 200,
						Label = new ChartLabel
						{
							ColumnKey = "Time"
						},
						Type = ChartType.Line,
						QueryKey = "VcsCommitOperations"
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 14,
							Bold = true
						},
						Text = "Chart F. Commit Branch VCS operations duration",
						HorizontalAlign = LabelHorizontalAlign.Center
					}
				}
			};

			return block;
		}

		public static IReportBlock GetTable(string queryKey, string tableLabel) =>
			new ContainerReportBlock
			{
				Childs = new IReportBlock[]
				{
					new TableReportBlock
					{
						BorderPx = 1,
						QueryKey = queryKey
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 14,
							Bold = true
						},
						Text = tableLabel,
						HorizontalAlign = LabelHorizontalAlign.Center
					}
				}
			};

		[Test]
		public void GenerateVcsTelemetryReport()
		{
			var reportRule = new ReportRule
			{
				Parameters = new ReportParameter[0],
				ReportTitle = "VCS telemetry",
				QueryLinks =
					new IReportQuery[]
					{
						new ReportQuery
						{
							Key = "VcsOperations",
							Text = @"VcsPluginTelemetry
select
DateTimeLocal=#DateTimeLocal.ToString()#
#DisplayName#
#OperationName#
OperationDuration = #OperationDuration# / 1000
#UserLogin#
#CreatedBranchName#
#CommittedSize#
select end"
						},
						new ReportQuery
						{
							Key = "VcsOperationsByName",
							Text = @"VcsPluginTelemetry
select
#OperationName#
select end
group #OperationName#
select
Operation = #Key.OperationName#
Count = #Count()#
select end
order #Count# desc #Operation# asc"
						},
						new ReportQuery
						{
							Key = "VcsOperationsByType",
							Text = @"VcsPluginTelemetry
select
#DisplayName#
select end
group #DisplayName#
select
Type = #Key.DisplayName#
Count = #Count()#
select end
order #Count# desc #Type# asc"
						},
						new ReportQuery
						{
							Key = "VcsCommitOperations",
							Text = @"VcsPluginTelemetry
where #OperationName# == ""commit""
order #DateTimeUtc# asc
select
Time = #DateTimeUtc.ToString()#
OperationDuration = #OperationDuration# / 1000
select end"
						},
						new ReportQuery
						{
							Key = "VcsCreateBranchOperations",
							Text = @"VcsPluginTelemetry
where #OperationName# == ""create-branch""
order #DateTimeUtc# asc
select
Time = #DateTimeUtc.ToString()#
OperationDuration = #OperationDuration# / 1000
select end"
						},
						new ReportQuery
						{
							Key = "VcsCheckoutOperations",
							Text = @"VcsPluginTelemetry
where #OperationName# == ""checkout""
order #DateTimeUtc# asc
select
Time = #DateTimeUtc.ToString()#
OperationDuration = #OperationDuration# / 1000
select end"
						}
					},
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
								GetHeader(),
								GetDohnuts(),
								GetCharts(),
								GetTable("VcsOperations", "Table 1. VCS operations")
							}
						}
					}
				}
			};

			var serializedRule = reportRule.ToJson();

			serializedRule.Should().NotBeNullOrEmpty();
		}
	}
}