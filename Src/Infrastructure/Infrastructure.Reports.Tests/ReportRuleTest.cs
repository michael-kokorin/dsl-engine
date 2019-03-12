namespace Infrastructure.Reports.Tests
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
	public sealed class ReportRuleTest
	{
		[Test]
		public void Get()
		{
			var reportRule = new ReportRule
			{
				Parameters = new ReportParameter[0],
				QueryLinks = new IReportQuery[0],
				Template = new ReportTemplate
				{
					Root = new HtmlDocReportBlock
					{
						Child = new ContainerReportBlock("ContainerMain")
						{
							Childs = new IReportBlock[]
							{
								new LabelReportBlock
								{
									Text = "Test"
								},
								new ContainerReportBlock
								{
									BreakPageBefore = true,
									Childs = new IReportBlock[]
									{
										new LabelReportBlock
										{
											Text = "Second"
										}
									}
								}
							}
						}
					}
				}
			};

			var serialized = reportRule.ToJson();

			serialized.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void ShouldSerializeReportRule()
		{
			var reportRule = new ReportRule
			{
				Parameters = new [] {new ReportParameter("Project Id", "ProjectId", "1") },
				QueryLinks = new IReportQuery[]
				{
					new ReportQueryLink("Task", 51)
				},
				Template = new ReportTemplate
				{
					Root = new HtmlDocReportBlock(true)
					{
						Child = new ContainerReportBlock
						{
							Childs = new IReportBlock[]
							{
								new LabelReportBlock
								{
									HorizontalAlign = LabelHorizontalAlign.Center,
									FontStyle = new LabelFontStyle(46),
									Text = "AI SDL Report: Scan tasks"
								},
								new LabelReportBlock
								{
									HorizontalAlign = LabelHorizontalAlign.Center,
									FontStyle = new LabelFontStyle(28),
									Text = "AI core work time chart"
								},
								new LabelReportBlock
								{
									HorizontalAlign = LabelHorizontalAlign.Center,
									FontStyle = new LabelFontStyle(16),
									Text = "Report prepared: $CurrentDate$"
								},
								new ContainerReportBlock
								{
									Childs = new IReportBlock[]
									{
										new ContainerReportBlock
										{
											Childs = new IReportBlock[]
											{
												new LabelReportBlock
												{
													FontStyle = new LabelFontStyle(18),
													HorizontalAlign = LabelHorizontalAlign.Center,
													Text = "Chart 1: Bar chart",
													VerticalAlign = LabelVerticalalign.Middle
												},
												new ChartReportBlock
												{
													Columns = new[]
													{
														new ChartColumn
														{
															ColumnKey = "WorkTime",
															DisplayName = "Scan core working time [s]"
														},
														new ChartColumn
														{
															ColumnKey = "Size",
															DisplayName = "Sources size [kb]"
														}
													},
													HeightPx = 500,
													Label = new ChartLabel
													{
														ColumnKey = "Created"
													},
													Type = ChartType.Bar,
													QueryKey = "Task"
												}
											},
											Orientation = ContainerOrientation.Vertical
										},
										new ContainerReportBlock
										{
											Childs = new IReportBlock[]
											{
												new LabelReportBlock
												{
													FontStyle = new LabelFontStyle(18),
													HorizontalAlign = LabelHorizontalAlign.Center,
													Text = "Chart 2: Line graph",
													VerticalAlign = LabelVerticalalign.Middle
												},
												new ChartReportBlock
												{
													Columns = new[]
													{
														new ChartColumn
														{
															ColumnKey = "WorkTime",
															DisplayName = "Scan core working time"
														},
														new ChartColumn
														{
															ColumnKey = "Size",
															DisplayName = "Sources size"
														}
													},
													HeightPx = 500,
													Label = new ChartLabel
													{
														ColumnKey = "Created"
													},
													Type = ChartType.Line,
													QueryKey = "Task"
												}
											},
											Orientation = ContainerOrientation.Vertical
										}
									},
									Orientation = ContainerOrientation.Horizontal
								},
								new TableReportBlock
								{
									BorderPx = 1,
									QueryKey = "Task"
								},
								new LabelReportBlock
								{
									FontStyle = new LabelFontStyle(12),
									HorizontalAlign = LabelHorizontalAlign.Center,
									Text = "System version: $SystemVersion$"
								}
							},
							Orientation = ContainerOrientation.Vertical
						}
					}
				}
			};

			var serialized = reportRule.ToJson();

			serialized.Should().NotBeNullOrEmpty();
		}
	}
}