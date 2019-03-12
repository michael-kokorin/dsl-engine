namespace Infrastructure.Reports.Tests.ReportSamples
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Reports.Blocks;
	using Infrastructure.Reports.Blocks.Chart;
	using Infrastructure.Reports.Blocks.Container;
	using Infrastructure.Reports.Blocks.HtmlDoc;
	using Infrastructure.Reports.Blocks.Iterator;
	using Infrastructure.Reports.Blocks.Label;
	using Infrastructure.Reports.Blocks.Table;

	[TestFixture]
	public sealed class FtpTechReportTest
	{
		[Test]
		public void ShouldCreateFtpTechReport()
		{
			var reportRule = new ReportRule
			{
				Parameters = new[]
				{
					new ReportParameter("Project identifier", "ProjectId", "1")
				},
				QueryLinks = new IReportQuery[]
				{
					new ReportQuery
					{
						Key = "Project",
						Text = @"Projects
where #Id# == {ProjectId}
select
#DisplayName#
#DefaultBranchName#
Created = #Created.ToString()#
Modified = #Modified.ToString()#
VcsPlugin = #Plugins1.DisplayName#
ItPlugin = #Plugins?.DisplayName#
#VcsLastSyncUtc#
select end"
					},
					new ReportQuery
					{
						Key = "ProjectTasks",
						Text = @"Tasks
where #ProjectId# == {ProjectId}
order #Created# asc
select
Created = #Created.ToString()#
Finished = #Finished.ToString()#
StartedBy = #Users.DisplayName#
High = #HighSeverityVulns#
Med = #MediumSeverityVulns#
Low = #LowSeverityVulns#
Ldap = #TaskResults.Count(r => r.TypeShort == ""LDAP"")#
Xss = #TaskResults.Count(r => r.TypeShort == ""XSS"")#
Total = #HighSeverityVulns# + #MediumSeverityVulns# + #LowSeverityVulns#
FolderSize = #FolderSize# / 1024
AnalyzedSize = #AnalyzedSize# / 1024 / 1024
#Todo#
#Reopen#
#Fixed#
#FP#
select end"
					}
				},
				ReportTitle = "AI SSDL technical report",
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
								GetFirstPart(),
								GetSecondPart(),
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

		private static IReportBlock GetFirstPart()
		{
			var block = new ContainerReportBlock("ContainerTaskInfo")
			{
				BackgroundColor = new ReportItemColor(236, 236, 236),
				Childs = new IReportBlock[]
				{
					new IteratorReportBlock
					{
						Id = "IteratorProjectInfo",
						QueryKey = "Project",
						Child = new ContainerReportBlock("ContainerTaskStatHead")
						{
							BackgroundColor = new ReportItemColor(236, 236, 236),
							ChildProportions = new[] {20, 80},
							Childs = new IReportBlock[]
							{
								new ContainerReportBlock("ContainerTaskStatHeadLeft")
								{
									BackgroundColor = new ReportItemColor(236, 236, 236),
									Childs = new IReportBlock[]
									{
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "Project name:"
										},
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "Default branch:"
										},
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "Created:"
										},
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "Modified:"
										},
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "Version control plugin:"
										},
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "Issue tracker plugin:"
										},
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "Latest auto sync:"
										}
									},
									Orientation = ContainerOrientation.Vertical
								},
								new ContainerReportBlock("ContainerTaskStatHeadRight")
								{
									BackgroundColor = new ReportItemColor(236, 236, 236),
									Childs = new IReportBlock[]
									{
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												Bold = true,
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "$ProjectItem.DisplayName$"
										},
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												Bold = true,
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "$ProjectItem.DefaultBranchName$"
										},
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												Bold = true,
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "UTC $ProjectItem.Created$"
										},
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												Bold = true,
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "UTC $ProjectItem.Modified$"
										},
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												Bold = true,
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "$ProjectItem.VcsPlugin$"
										},
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												Bold = true,
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "$ProjectItem.ItPlugin$"
										},
										new LabelReportBlock
										{
											FontStyle = new LabelFontStyle
											{
												Bold = true,
												FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
												FontSizePx = 12
											},
											Text = "$ProjectItem.VcsLastSyncUtc$"
										}
									},
									Orientation = ContainerOrientation.Vertical
								}
							},
							Orientation = ContainerOrientation.Horizontal
						}
					}
				},
				MarginTopPx = 10,
				PaddingRightPx = 30,
				PaddingLeftPx = 80,
				PaddingTopPx = 15,
				PaddingBottomPx = 15,
				Id = "ContainerScanInfo",
				Orientation = ContainerOrientation.Vertical
			};

			return block;
		}

		private static IReportBlock GetSecondPart()
		{
			var block = new ContainerReportBlock("DoughnutsContainer")
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
							ColumnKey = "Created"
						},
						Type = ChartType.Line,
						QueryKey = "ProjectTasks"
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
					new ChartReportBlock
					{
						Columns = new[]
						{
							new ChartColumn
							{
								ColumnKey = "Todo",
								DisplayName = "TO DO"
							},
							new ChartColumn
							{
								ColumnKey = "Reopen",
								DisplayName = "Reopened"
							},
							new ChartColumn
							{
								ColumnKey = "Fixed",
								DisplayName = "Fixed"
							},
							new ChartColumn
							{
								ColumnKey = "FP",
								DisplayName = "False positive"
							}
						},
						HeightPx = 250,
						Label = new ChartLabel
						{
							ColumnKey = "Created"
						},
						Type = ChartType.Line,
						QueryKey = "ProjectTasks"
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
						Text = "Chart 2. Vulnerabilities distribution by annotation",
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
								ColumnKey = "Total",
								DisplayName = "Total"
							},
							new ChartColumn
							{
								ColumnKey = "Xss",
								DisplayName = "XSS"
							},
							new ChartColumn
							{
								ColumnKey = "Ldap",
								DisplayName = "LDAP"
							}
						},
						HeightPx = 250,
						Label = new ChartLabel
						{
							ColumnKey = "Created"
						},
						Type = ChartType.Line,
						QueryKey = "ProjectTasks"
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
						Text = "Chart 3. Vulnerabilities distribution by type",
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
							ColumnKey = "Created"
						},
						Type = ChartType.Line,
						QueryKey = "ProjectTasks"
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
						Text = "Chart 4. Scan process counters",
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
						QueryKey = "ProjectTasks"
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
						Text = "Table 1. Scan task results summary",
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