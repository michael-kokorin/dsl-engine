namespace Infrastructure.Reports.Tests.ReportSamples
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
	public sealed class FtpReportTest
	{
		[Test]
		public void ShouldCreateFtpReport()
		{
			var reportRule = new ReportRule
			{
				Parameters = new[]
				{
					new ReportParameter("Vulnerability type", "VulnType", "XSS")
				},
				QueryLinks = new IReportQuery[]
				{
					new ReportQueryLink("Projects", 36),
					new ReportQueryLink("ProjectsVulnTypeSummary", 37),
					new ReportQueryLink("VulnTypeSummary", 38),
					new ReportQueryLink("CompliantVulnerabilitiesAll", 41)
					{
						Parameters = new []
						{
							new ReportQueryParameter("SdlStatus", "1")
						}
					},
					new ReportQueryLink("NonCompliantVulnerabilitiesAll", 41)
					{
						Parameters = new []
						{
							new ReportQueryParameter("SdlStatus", "2")
						}
					},
					new ReportQueryLink("CompliantProjects", 42)
					{
						Parameters = new[]
						{
							new ReportQueryParameter("SdlStatus", "1")
						}
					},
					new ReportQueryLink("NoncompliantProjects", 42)
					{
						Parameters = new[]
						{
							new ReportQueryParameter("SdlStatus", "2")
						}
					},
					new ReportQueryLink("Top5CompliantVulnerabilitiesAll", 43)
					{
						Parameters = new []
						{
							new ReportQueryParameter("SdlStatus", "1")
						}
					},
					new ReportQueryLink("Top5NonCompliantVulnerabilitiesAll", 43)
					{
						Parameters = new []
						{
							new ReportQueryParameter("SdlStatus", "2")
						}
					}
				},
				ReportTitle = "AI SSDL analyst report",
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
								GetCompiant(),
								GetNoncompiant(),
								GetFirstPart(),
								GetAfterParty()
							}
						}
					}
				}
			};

			var serializedRule = reportRule.ToJson();

			serializedRule.Should().NotBeNullOrEmpty();
		}

		private static IReportBlock GetHeader()
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

		private static IReportBlock GetCompiant()
		{
			var block = new ContainerReportBlock("ConpliantBlock")
			{
				Orientation = ContainerOrientation.Vertical,
				Childs = new IReportBlock[]
				{
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 18,
							Bold = true
						},
						Id = "LabelCompliantPart",
						Text = "Compliant project vulnerabilities",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new TableReportBlock
					{
						BorderPx = 1,
						Id = "CompliantProjects",
						QueryKey = "CompliantProjects"
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 14,
							Bold = true
						},
						Id = "LabelTableCompliantProjects",
						Text = "Table CP. Compliant projects",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					},
					new ContainerReportBlock("DoughnutsContainer")
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
											ColumnKey = "TypeShort"
										},
										Type = ChartType.Doughnut,
										QueryKey = "CompliantVulnerabilitiesAll"
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
										Text = "Chart C. Vulnerability types in compliant projects",
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
											ColumnKey = "TypeShort"
										},
										Type = ChartType.Doughnut,
										QueryKey = "Top5CompliantVulnerabilitiesAll"
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
										Text = "Chart CT. TOP-5 Vulnerability types in compliant projects",
										HorizontalAlign = LabelHorizontalAlign.Center
									}
								}
							}
						}
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					},
					new TableReportBlock
					{
						BorderPx = 1,
						Id = "CompliantVulnerabilities",
						QueryKey = "CompliantVulnerabilitiesAll"
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 14,
							Bold = true
						},
						Id = "LabelCompliantVulns",
						Text = "Table C. Vulnerability types in compliant projects",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					}
				}
			};

			return block;
		}

		private static IReportBlock GetNoncompiant()
		{
			var block = new ContainerReportBlock("ConpliantBlock")
			{
				Orientation = ContainerOrientation.Vertical,
				Childs = new IReportBlock[]
				{
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 18,
							Bold = true
						},
						Id = "LabelFirstPartName",
						Text = "Noncompliant project vulnerabilities",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new TableReportBlock
					{
						BorderPx = 1,
						Id = "TableNoncompliantProjects",
						QueryKey = "NoncompliantProjects"
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 14,
							Bold = true
						},
						Id = "LabelTableNoncompliantProjects",
						Text = "Table NP. Noncompliant projects",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					},
					new ContainerReportBlock("DoughnutsContainer")
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
											ColumnKey = "TypeShort"
										},
										Type = ChartType.Doughnut,
										QueryKey = "NonCompliantVulnerabilitiesAll"
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
										Text = "Chart NC. Vulnerability types in noncompliant projects",
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
											ColumnKey = "TypeShort"
										},
										Type = ChartType.Doughnut,
										QueryKey = "Top5NonCompliantVulnerabilitiesAll"
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
										Text = "Chart NCT. TOP-5 Vulnerability types in noncompliant projects",
										HorizontalAlign = LabelHorizontalAlign.Center
									}
								}
							}
						}
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					},
					new TableReportBlock
					{
						BorderPx = 1,
						Id = "CompliantVulnerabilities",
						QueryKey = "NoncompliantVulnerabilitiesAll"
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 14,
							Bold = true
						},
						Id = "LabelNoncompliantVulns",
						Text = "Table N. Vulnerability types in compliant projects",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					}
				}
			};

			return block;
		}

		private static IReportBlock GetFirstPart()
		{
			var block = new ContainerReportBlock("FirstPartContainer")
			{
				Orientation = ContainerOrientation.Vertical,
				BreakPageBefore = true,
				Childs = new IReportBlock[]
				{
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 18,
							Bold = true
						},
						Id = "LabelFirstPartName",
						Text = "Scan results summary",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ChartReportBlock
					{
						Columns = new[]
						{
							new ChartColumn
							{
								ColumnKey = "High",
								DisplayName = "High vulnerability severity"
							},
							new ChartColumn
							{
								ColumnKey = "Med",
								DisplayName = "Medium vulnerability sevirity"
							},
							new ChartColumn
							{
								ColumnKey = "Low",
								DisplayName = "Low vulnerability sevirity"
							}
						},
						HeightPx = 250,
						Label = new ChartLabel
						{
							ColumnKey = "ProjectName"
						},
						Type = ChartType.Bar,
						QueryKey = "Projects"
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
						Text = "Chart 1. Vulnerabilities distribution by severity",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					},
					new ContainerReportBlock("DoughnutsContainer")
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
												ColumnKey = "High",
												DisplayName = "High vulnerability severity"
											}
										},
										HeightPx = 250,
										Label = new ChartLabel
										{
											ColumnKey = "ProjectName"
										},
										Type = ChartType.Doughnut,
										QueryKey = "Projects"
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
										Text = "Chart 2. Hign severity",
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
												ColumnKey = "Med",
												DisplayName = "High vulnerability severity"
											}
										},
										HeightPx = 250,
										Label = new ChartLabel
										{
											ColumnKey = "ProjectName"
										},
										Type = ChartType.Doughnut,
										QueryKey = "Projects"
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
										Text = "Chart 3. Medium severity",
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
												ColumnKey = "Low",
												DisplayName = "High vulnerability severity"
											}
										},
										HeightPx = 250,
										Label = new ChartLabel
										{
											ColumnKey = "ProjectName"
										},
										Type = ChartType.Doughnut,
										QueryKey = "Projects"
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
										Text = "Chart 4. Low severity",
										HorizontalAlign = LabelHorizontalAlign.Center
									}
								}
							}
						}
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					},
					new ChartReportBlock
					{
						Columns = new[]
						{
							new ChartColumn
							{
								ColumnKey = "Count",
								DisplayName = "Vulnerabilities"
							}
						},
						HeightPx = 250,
						Label = new ChartLabel
						{
							ColumnKey = "Project"
						},
						Type = ChartType.Doughnut,
						QueryKey = "VulnTypeSummary"
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
						Text = "Chart 5. $VulnType$ vulnerability type distribution",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					},
					new ChartReportBlock
					{
						Columns = new[]
						{
							new ChartColumn
							{
								ColumnKey = "Todo",
								DisplayName = "To do"
							},
							new ChartColumn
							{
								ColumnKey = "Reopened",
								DisplayName = "Reopened"
							},
							new ChartColumn
							{
								ColumnKey = "FP",
								DisplayName = "False positive"
							},
							new ChartColumn
							{
								ColumnKey = "Fixed",
								DisplayName = "Fixed"
							}
						},
						HeightPx = 250,
						Label = new ChartLabel
						{
							ColumnKey = "ProjectName"
						},
						Type = ChartType.Bar,
						QueryKey = "Projects"
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 14,
							Bold = true
						},
						Id = "LabelPartOneDescription",
						Text = "Chart 6. Vulnerability annotations",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					},
					new ChartReportBlock
					{
						Columns = new[]
						{
							new ChartColumn
							{
								ColumnKey = "ScanCoreWorkingTime",
								DisplayName = "Scan core(s) working time [s]"
							}
						},
						HeightPx = 250,
						Label = new ChartLabel
						{
							ColumnKey = "ProjectName"
						},
						Type = ChartType.Bar,
						QueryKey = "Projects"
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
						Text = "Chart 7. Scan core(s) working time [seconds]",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					},
					new TableReportBlock
					{
						BorderPx = 1,
						QueryKey = "ProjectsVulnTypeSummary"
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 14,
							Bold = true
						},
						Id = "LabelPartOneDescription",
						Text = "Table 1. Found vulnerability types summary",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					},
					new ChartReportBlock
					{
						Columns = new[]
						{
							new ChartColumn
							{
								ColumnKey = "Ldap",
								DisplayName = "LDAP"
							},
							new ChartColumn
							{
								ColumnKey = "Xss",
								DisplayName = "XSS"
							}
						},
						HeightPx = 250,
						Label = new ChartLabel
						{
							ColumnKey = "ProjectName"
						},
						Type = ChartType.Bar,
						QueryKey = "Projects"
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
						Text = "Chart 7. LDAP and XSS vulnerabilities distribution",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					},
					new ChartReportBlock
					{
						Columns = new[]
						{
							new ChartColumn
							{
								ColumnKey = "FolderSize",
								DisplayName = "Folder size [KB]"
							},
							new ChartColumn
							{
								ColumnKey = "AnalyzedSize",
								DisplayName = "Analyzed size [KB]"
							}
						},
						HeightPx = 250,
						Label = new ChartLabel
						{
							ColumnKey = "ProjectName"
						},
						Type = ChartType.Bar,
						QueryKey = "Projects"
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
						Text = "Chart 8. Scan counters",
						HorizontalAlign = LabelHorizontalAlign.Center
					},
					new ContainerReportBlock
					{
						BackgroundColor = new ReportItemColor(236, 236, 236),
						MarginTopPx = 10,
						MarginBottomPx = 10,
						PaddingRightPx = 30,
						PaddingLeftPx = 80,
						PaddingTopPx = 15,
						PaddingBottomPx = 15,
						Childs = new IReportBlock[]
						{
							new LabelReportBlock
							{
								FontStyle = new LabelFontStyle
								{
									FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
									FontSizePx = 12,
									Bold = true
								},
								Id = "LabelChartOneDescription",
								Text = "Placeholder for a something intresting",
								HorizontalAlign = LabelHorizontalAlign.Center
							}
						}
					},
					new TableReportBlock
					{
						BorderPx = 1,
						QueryKey = "Projects"
					},
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle
						{
							FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
							FontSizePx = 14,
							Bold = true
						},
						Id = "LabelPartOneDescription",
						Text = "Table 2. Summary scan information",
						HorizontalAlign = LabelHorizontalAlign.Center
					}
				}
			};

			return block;
		}

		private static IReportBlock GetAfterParty() =>
			new ContainerReportBlock("AfterpatyContainer")
			{
				PaddingTopPx = 20,
				Childs = new IReportBlock[]
				{
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle(12),
						HorizontalAlign = LabelHorizontalAlign.Center,
						Text = "AI SSDL version: $SystemVersion$"
					}
				}
			};
	}
}