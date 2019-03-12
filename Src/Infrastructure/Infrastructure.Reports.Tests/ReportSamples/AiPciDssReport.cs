namespace Infrastructure.Reports.Tests.ReportSamples
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Reports.Blocks;
	using Infrastructure.Reports.Blocks.BoolReportBlock;
	using Infrastructure.Reports.Blocks.Container;
	using Infrastructure.Reports.Blocks.Html;
	using Infrastructure.Reports.Blocks.HtmlDoc;
	using Infrastructure.Reports.Blocks.Iterator;
	using Infrastructure.Reports.Blocks.Label;
	using Infrastructure.Reports.Blocks.Link;

	[TestFixture]
	public sealed class AiPciDssReport
	{
		[Test]
		public void ShouldGenerateAiPciDssReport()
		{
			var reportRule = new ReportRule
			{
				Parameters = new[]
				{
					new ReportParameter("Task Id", "TaskId", 1.ToString())
				},
				QueryLinks = new IReportQuery[]
				{
					new ReportQuery
					{
						Key = "ReportTask",
						Text = @"Tasks
where #Id# == {TaskId}
select
#Id#
ProjectName=#Projects.DisplayName#
#FolderPath#
Branch=#Repository#
StartedBy=#Users#.DisplayName
Started=#Created#
#Finished#
FolderSize=#FolderSize# ?? 0
AnalyzedSize=#AnalyzedSize# ?? 0
AnalyzedFiles=#AnalyzedFiles# ?? ""0""
AnalyzedLinesCount=#AnalyzedLinesCount# ?? 0
FoundTotal=(#Todo# + #Reopen# + #FP# + #Fixed#) ?? 0
FP=#FP# ?? 0
Todo=#Todo# ?? 0
Reopen=#Reopen# ?? 0
Fixed=#Fixed# ?? 0
#HighSeverityVulns#
HighPercent = #HighSeverityVulns# > 0 ? #HighSeverityVulns# * 100 / (#HighSeverityVulns# + #MediumSeverityVulns# + #LowSeverityVulns#) : 0
#MediumSeverityVulns#
MedPercent = #MediumSeverityVulns# > 0 ? #MediumSeverityVulns# * 100 / (#HighSeverityVulns# + #MediumSeverityVulns# + #LowSeverityVulns#) : 0
#LowSeverityVulns#
LowPercent = #LowSeverityVulns# > 0 ? #LowSeverityVulns# * 100 / (#HighSeverityVulns# + #MediumSeverityVulns# + #LowSeverityVulns#) : 0
select end"
					},
					new ReportQuery
					{
						Key = "ReportTaskResults",
						Text = @"TaskResults
where #TaskId# == {TaskId}
select
#Id#
#TaskId#
#Type#
#SeverityType#
IsHigh=#SeverityType# == 3
IsMedium=#SeverityType# == 2
IsLow=#SeverityType# == 1
#Function#
#File#
Query=#ExploitGraph#
Condition=#AdditionalExploitConditions#
#LineNumber#
#RawLine#
#IssueUrl#
select end
order #SeverityType# desc"
					}
				},
				Template = new ReportTemplate
				{
					Root = new HtmlDocReportBlock(true)
					{
						Width = "900px",
						Child = new ContainerReportBlock("ContainerMain")
						{
							PaddingRightPx = 30,
							PaddingLeftPx = 30,
							MarginTopPx = 8,
							MarginLeftPx = 8,
							MarginRightPx = 8,
							MarginBottomPx = 8,
							Childs = new IReportBlock[]
							{
								new ContainerReportBlock("ContainerHeader")
								{
									Childs = new IReportBlock[]
									{
										new ContainerReportBlock("ContainerReportInfo")
										{
											Childs = new IReportBlock[]
											{
												new ContainerReportBlock
												{
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
															Text = "Positive Technologies SDL"
														},
														new LabelReportBlock
														{
															FontStyle = new LabelFontStyle
															{
																FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																FontSizePx = 24
															},
															Id = "LabelReportHeaderReportName",
															Text = "Report:Scan Results"
														}
													},
													Id = "ContainerReportName",
													Orientation = ContainerOrientation.Vertical
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
											},
											Orientation = ContainerOrientation.Horizontal
										}
									},
									MarginLeftPx = 80,
									MarginTopPx = 35,
									MarginBottomPx = 0,
									MarginRightPx = 0
								},
								new ContainerReportBlock("ContainerTaskInfo")
								{
									BackgroundColor = new ReportItemColor(236, 236, 236),
									Childs = new IReportBlock[]
									{
										new IteratorReportBlock
										{
											Id = "IteratorTaskInfo",
											QueryKey = "ReportTask",
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
																Text = "Scan project:"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "Branch name:"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "Scan folder:"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "Scan core name:"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "Started by:"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "Task created:"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "Task finished:"
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
																Text = "$ReportTaskItem.ProjectName$"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	Bold = true,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "$ReportTaskItem.Branch$"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	Bold = true,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "$ReportTaskItem.FolderPath$"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	Bold = true,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "$TaskScanCore.Value$"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	Bold = true,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "$ReportTaskItem.StartedBy$"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	Bold = true,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "$ReportTaskItem.Started$"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	Bold = true,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "$ReportTaskItem.Finished$"
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
								},
								new IteratorReportBlock
								{
									Id = "IteratorTaskInfo",
									QueryKey = "ReportTask",
									Child = new ContainerReportBlock("ContainerSourceCodeStat")
									{
										Childs = new IReportBlock[]
										{
											new ContainerReportBlock("ContainerSourceCodeStatLeft")
											{
												Childs = new IReportBlock[]
												{
													new ContainerReportBlock("ContainerVulnerabilitiesSourceCodeCheckHeader")
													{
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	Bold = true,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 14
																},
																Text = "Source code check"
															}
														},
														Orientation = ContainerOrientation.Horizontal,
														MarginTopPx = 14,
														MarginBottomPx = 14
													},
													new ContainerReportBlock("ContainerVulnerabilitiesSourceCodeCheckSourceFolderSize")
													{
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
																Text = "Source folder size"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	Bold = true,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "$ReportTaskItem.FolderSize$ Kb",
																HorizontalAlign = LabelHorizontalAlign.Right
															}
														},
														Orientation = ContainerOrientation.Horizontal,
														PaddingTopPx = 1,
														PaddingBottomPx = 1
													},
													new ContainerReportBlock("ContainerVulnerabilitiesSourceCodeCheckSourceAnalyzed")
													{
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "Analyzed size"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "$ReportTaskItem.AnalyzedSize$ Kb",
																HorizontalAlign = LabelHorizontalAlign.Right
															}
														},
														Orientation = ContainerOrientation.Horizontal,
														PaddingTopPx = 1,
														PaddingBottomPx = 1
													},
													new ContainerReportBlock("ContainerVulnerabilitiesSourceCodeCheckSourceAnalyzedLines")
													{
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "Analyzed lines count"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "$ReportTaskItem.AnalyzedLinesCount$",
																HorizontalAlign = LabelHorizontalAlign.Right
															}
														},
														Orientation = ContainerOrientation.Horizontal,
														PaddingTopPx = 1,
														PaddingBottomPx = 1
													}
												},
												Orientation = ContainerOrientation.Vertical,
												PaddingLeftPx = 10,
												PaddingRightPx = 10,
												PaddingBottomPx = 15
											},
											new ContainerReportBlock("ContainerSourceCodeStatRight")
											{
												BackgroundColor = new ReportItemColor(217, 217, 217),
												Childs = new IReportBlock[]
												{
													new ContainerReportBlock("ContainerVulnerabilitiesFoundHeader")
													{
														BackgroundColor = new ReportItemColor(217, 217, 217),
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	Bold = true,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 14
																},
																Text = "Vulnerabilities found"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	Bold = true,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 14
																},
																Color = new ReportItemColor(189, 22, 34),
																Text = "$ReportTaskItem.FoundTotal$",
																HorizontalAlign = LabelHorizontalAlign.Right
															}
														},
														Orientation = ContainerOrientation.Horizontal,
														MarginTopPx = 14,
														MarginBottomPx = 14
													},
													new ContainerReportBlock("ContainerVulnerabilitiesFoundTodo")
													{
														BackgroundColor = new ReportItemColor(217, 217, 217),
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "To do"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "$ReportTaskItem.Todo$",
																HorizontalAlign = LabelHorizontalAlign.Right
															}
														},
														Orientation = ContainerOrientation.Horizontal,
														PaddingTopPx = 1,
														PaddingBottomPx = 1
													},
													new ContainerReportBlock("ContainerVulnerabilitiesFoundReopened")
													{
														BackgroundColor = new ReportItemColor(217, 217, 217),
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "Reopened"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "$ReportTaskItem.Reopen$",
																HorizontalAlign = LabelHorizontalAlign.Right
															}
														},
														Orientation = ContainerOrientation.Horizontal,
														PaddingTopPx = 1,
														PaddingBottomPx = 1
													},
													new ContainerReportBlock("ContainerVulnerabilitiesFoundFalsePositive")
													{
														BackgroundColor = new ReportItemColor(217, 217, 217),
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "False positive"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "$ReportTaskItem.FP$",
																HorizontalAlign = LabelHorizontalAlign.Right
															}
														},
														Orientation = ContainerOrientation.Horizontal,
														PaddingTopPx = 1,
														PaddingBottomPx = 1
													},
													new ContainerReportBlock("ContainerVulnerabilitiesFoundFixed")
													{
														BackgroundColor = new ReportItemColor(217, 217, 217),
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "Fixed"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif",
																	FontSizePx = 12
																},
																Text = "$ReportTaskItem.Fixed$",
																HorizontalAlign = LabelHorizontalAlign.Right
															}
														},
														Orientation = ContainerOrientation.Horizontal,
														PaddingTopPx = 1,
														PaddingBottomPx = 1
													}
												},
												Orientation = ContainerOrientation.Vertical,
												PaddingLeftPx = 10,
												PaddingRightPx = 10,
												PaddingBottomPx = 15
											}
										},
										PaddingLeftPx = 80,
										MarginTopPx = 15,
										Orientation = ContainerOrientation.Horizontal
									}
								},
								new IteratorReportBlock
								{
									Id = "IteratorTaskInfo",
									QueryKey = "ReportTask",
									Child = new ContainerReportBlock("ContainerVulnerabilityDistribution")
									{
										Childs = new IReportBlock[]
										{
											new ContainerReportBlock("ContainerVulnerabilityDistributionHeader")
											{
												Childs = new IReportBlock[]
												{
													new LabelReportBlock
													{
														FontStyle = new LabelFontStyle
														{
															Bold = true,
															FontSizePx = 24,
															FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
														},
														Id = "LabelVulnerabilityDistribution",
														Text = "Vulnerability distribution"
													}
												},
												PaddingLeftPx = 80,
												MarginTopPx = 24,
												MarginBottomPx = 24
											},
											new ContainerReportBlock("ContainerVulnerabilityDistributionLevels")
											{
												BackgroundColor = new ReportItemColor(236, 236, 236),
												PaddingLeftPx = 80,
												PaddingTopPx = 10,
												PaddingBottomPx = 10,
												Childs = new IReportBlock[]
												{
													new ContainerReportBlock("ContainerVulnerabilityDistributionLevelsByLevel")
													{
														BackgroundColor = new ReportItemColor(236, 236, 236),
														MarginBottomPx = 5,
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	Bold = true,
																	FontSizePx = 16,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "By levels"
															}
														}
													},
													new ContainerReportBlock("ContainerVulnerabilityDistributionLevelsByLevelHigh")
													{
														BackgroundColor = new ReportItemColor(236, 236, 236),
														Orientation = ContainerOrientation.Horizontal,
														MarginTopPx = 2,
														MarginBottomPx = 2,
														ChildProportions = new[] {20, 10, 70},
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontSizePx = 12,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "High"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontSizePx = 12,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "$ReportTaskItem.HighSeverityVulns$"
															},
															new HtmlReportBlock
															{
																Template =
																	"<div style='background-color: #F98A73; height: 12px; width: $ReportTaskItem.HighPercent$%'></div>"
															}
														}
													},
													new ContainerReportBlock("ContainerVulnerabilityDistributionLevelsByLevelMed")
													{
														BackgroundColor = new ReportItemColor(236, 236, 236),
														Orientation = ContainerOrientation.Horizontal,
														MarginTopPx = 2,
														MarginBottomPx = 2,
														ChildProportions = new[] {20, 10, 70},
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontSizePx = 12,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "Medium"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontSizePx = 12,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "$ReportTaskItem.MediumSeverityVulns$"
															},
															new HtmlReportBlock
															{
																Template =
																	"<div style='background-color: #F9BF51; height: 12px; width: $ReportTaskItem.MedPercent$%'></div>"
															}
														}
													},
													new ContainerReportBlock("ContainerVulnerabilityDistributionLevelsByLevelLow")
													{
														BackgroundColor = new ReportItemColor(236, 236, 236),
														Orientation = ContainerOrientation.Horizontal,
														MarginTopPx = 2,
														MarginBottomPx = 2,
														ChildProportions = new[] {20, 10, 70},
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontSizePx = 12,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "Low"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontSizePx = 12,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "$ReportTaskItem.LowSeverityVulns$"
															},
															new HtmlReportBlock
															{
																Template =
																	"<div style='background-color: #A9C1E3; height: 12px; width: $ReportTaskItem.LowPercent$%'></div>"
															}
														}
													}
												}
											},
											new ContainerReportBlock("ContainerVulnerabilityDistributionRequirements")
											{
												PaddingLeftPx = 80,
												PaddingTopPx = 10,
												PaddingBottomPx = 10,
												Childs = new IReportBlock[]
												{
													new ContainerReportBlock("ContainerDistributionByReqirementsHeader")
													{
														MarginBottomPx = 5,
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	Bold = true,
																	FontSizePx = 16,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "By PCI DSS requirements"
															}
														}
													},
													new ContainerReportBlock("ContainerDistributionByReqirementsPos1")
													{
														Orientation = ContainerOrientation.Horizontal,
														MarginTopPx = 2,
														MarginBottomPx = 2,
														ChildProportions = new[] {30, 10, 60},
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontSizePx = 12,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "6.5.1 - Cross-site scripting (XSS)"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontSizePx = 12,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "3"
															},
															new HtmlReportBlock
															{
																Template =
																	"<div style='background-color: #A9A9A9; height: 12px; width: 18%'></div>"
															}
														}
													},
													new ContainerReportBlock("ContainerDistributionByReqirementsPos2")
													{
														Orientation = ContainerOrientation.Horizontal,
														MarginTopPx = 2,
														MarginBottomPx = 2,
														ChildProportions = new[] {30, 10, 60},
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontSizePx = 12,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "6.5.5 - Cross-site request forgery (CSRF)"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontSizePx = 12,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "5"
															},
															new HtmlReportBlock
															{
																Template =
																	"<div style='background-color: #A9A9A9; height: 12px; width: 31%'></div>"
															}
														}
													},
													new ContainerReportBlock("ContainerDistributionByReqirementsPos3")
													{
														Orientation = ContainerOrientation.Horizontal,
														MarginTopPx = 2,
														MarginBottomPx = 2,
														ChildProportions = new[] {30, 10, 60},
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontSizePx = 12,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "6.5.8 - Insecure cryptographic storage"
															},
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontSizePx = 12,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text = "8"
															},
															new HtmlReportBlock
															{
																Template =
																	"<div style='background-color: #A9A9A9; height: 12px; width: 50%'></div>"
															}
														}
													}
												}
											}
										},
										Orientation = ContainerOrientation.Vertical
									}
								},
								new ContainerReportBlock("ContainerFoundVulnerabilities")
								{
									BreakPageBefore = true,
									Childs = new IReportBlock[]
									{
										new ContainerReportBlock("ContainerFoundVulnerabilitiesFound")
										{
											Childs = new IReportBlock[]
											{
												new LabelReportBlock
												{
													FontStyle = new LabelFontStyle
													{
														Bold = true,
														FontSizePx = 24,
														FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
													},
													Id = "LabelFoundVulnerabilities",
													Text = "Found vulnerabilities"
												},
												new LabelReportBlock
												{
													FontStyle = new LabelFontStyle
													{
														FontSizePx = 12,
														FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
													},
													Id = "LabelFoundVulnerabilitiesSub",
													Text = "All vulnerabilities that was found during the scan process"
												}
											},
											PaddingLeftPx = 80,
											MarginTopPx = 24,
											MarginBottomPx = 24
										},
										new IteratorReportBlock
										{
											Child = new ContainerReportBlock("ContainerVulnerabilityBlock")
											{
												BackgroundColor = new ReportItemColor(236, 236, 236),
												MarginBottomPx = 15,
												PaddingBottomPx = 10,
												Childs = new IReportBlock[]
												{
													new ContainerReportBlock
													{
														BackgroundColor = new ReportItemColor(236, 236, 236),
														ChildProportions = new[] {10, 90},
														Childs = new IReportBlock[]
														{
															new LabelReportBlock
															{
																FontStyle = new LabelFontStyle
																{
																	FontSizePx = 10,
																	FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																},
																Text =
																	"$if(ReportTaskResultsItem.IsHigh)$Hign$elseif(ReportTaskResultsItem.IsMedium)$Medium$elseif(ReportTaskResultsItem.IsLow)$Low$endif$",
																VerticalAlign = LabelVerticalalign.Middle,
																HorizontalAlign = LabelHorizontalAlign.Center
															},
															new ContainerReportBlock
															{
																BackgroundColor = new ReportItemColor(236, 236, 236),
																Childs = new IReportBlock[]
																{
																	new ContainerReportBlock
																	{
																		BackgroundColor = new ReportItemColor(236, 236, 236),
																		ChildProportions = new[] {70,70, 70, 30},
																		Childs = new IReportBlock[]
																		{
																			new BoolReportBlock
																			{
																				Positive = new ContainerReportBlock
																				{
																					MarginLeftPx = 10,
																					PaddingLeftPx = 10,
																					PaddingRightPx = 10,
																					PaddingTopPx = 1,
																					PaddingBottomPx = 1,
																					BackgroundColor = new ReportItemColor(249, 138, 115),
																					Childs = new IReportBlock[]
																					{
																						new LabelReportBlock
																						{
																							Color = new ReportItemColor(255, 255, 255),
																							FontStyle = new LabelFontStyle
																							{
																								FontSizePx = 14,
																								FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																							},
																							Text = "$ReportTaskResultsItem.Type$"
																						}
																					}
																				},

																				Variable = "ReportTaskResultsItem.IsHigh"
																			},
																			new BoolReportBlock
																			{
																				Positive = new ContainerReportBlock
																				{
																					MarginLeftPx = 10,
																					PaddingLeftPx = 10,
																					PaddingRightPx = 10,
																					PaddingTopPx = 1,
																					PaddingBottomPx = 1,
																					BackgroundColor = new ReportItemColor(249, 191, 81),
																					Childs = new IReportBlock[]
																					{
																						new LabelReportBlock
																						{
																							Color = new ReportItemColor(255, 255, 255),
																							FontStyle = new LabelFontStyle
																							{
																								FontSizePx = 14,
																								FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																							},
																							Text = "$ReportTaskResultsItem.Type$"
																						}
																					}
																				},
																				Variable = "ReportTaskResultsItem.IsMedium"
																			},
																			new BoolReportBlock
																			{
																				Positive = new ContainerReportBlock
																				{
																					MarginLeftPx = 10,
																					PaddingLeftPx = 10,
																					PaddingRightPx = 10,
																					PaddingTopPx = 1,
																					PaddingBottomPx = 1,
																					BackgroundColor = new ReportItemColor(169, 193, 227),
																					Childs = new IReportBlock[]
																					{
																						new LabelReportBlock
																						{
																							Color = new ReportItemColor(255, 255, 255),
																							FontStyle = new LabelFontStyle
																							{
																								FontSizePx = 14,
																								FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																							},
																							Text = "$ReportTaskResultsItem.Type$"
																						}
																					}
																				},
																				Variable = "ReportTaskResultsItem.IsLow"
																			},
																			new ContainerReportBlock
																			{
																				BackgroundColor = new ReportItemColor(236, 236, 236)
																			}
																		},
																		Orientation = ContainerOrientation.Horizontal
																	},
																	new ContainerReportBlock
																	{
																		BackgroundColor = new ReportItemColor(236, 236, 236),
																		MarginTopPx = 5,
																		MarginLeftPx = 20,
																		PaddingRightPx = 10,
																		ChildProportions = new[] {20, 5, 75},
																		Childs = new IReportBlock[]
																		{
																			new LabelReportBlock
																			{
																				FontStyle = new LabelFontStyle
																				{
																					FontSizePx = 12,
																					FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																				},
																				Text = "Vulnerability code:",
																				VerticalAlign = LabelVerticalalign.Middle
																			},
																			new LabelReportBlock
																			{
																				FontStyle = new LabelFontStyle
																				{
																					Bold = true,
																					FontSizePx = 12,
																					FontFamily = "'Myriad Pro Semibold', 'Segoe UI Semibold', 'Arial Semibold', 'sans-serif semibold'"
																				},
																				Text = "$ReportTaskResultsItem.LineNumber$",
																				VerticalAlign = LabelVerticalalign.Middle
																			},
																			new LabelReportBlock
																			{
																				Color = new ReportItemColor(189, 22, 34),
																				FontStyle = new LabelFontStyle
																				{
																					Bold = true,
																					FontSizePx = 12,
																					FontFamily = "'Myriad Pro Semibold', 'Segoe UI Semibold', 'Arial Semibold', 'sans-serif semibold'"
																				},
																				Text = "$ReportTaskResultsItem.RawLine$",
																				VerticalAlign = LabelVerticalalign.Middle
																			}
																		},
																		Orientation = ContainerOrientation.Horizontal
																	},
																	new ContainerReportBlock
																	{
																		BackgroundColor = new ReportItemColor(236, 236, 236),
																		MarginTopPx = 5,
																		MarginLeftPx = 20,
																		PaddingBottomPx = 5,
																		PaddingRightPx = 10,
																		ChildProportions = new[] {20, 80},
																		Childs = new IReportBlock[]
																		{
																			new LabelReportBlock
																			{
																				FontStyle = new LabelFontStyle
																				{
																					FontSizePx = 12,
																					FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																				},
																				Text = "Function:",
																				VerticalAlign = LabelVerticalalign.Middle
																			},
																			new LabelReportBlock
																			{
																				FontStyle = new LabelFontStyle
																				{
																					Bold = true,
																					FontSizePx = 12,
																					FontFamily = "'Myriad Pro Semibold', 'Segoe UI Semibold', 'Arial Semibold', 'sans-serif semibold'"
																				},
																				Text = "$ReportTaskResultsItem.Function$",
																				VerticalAlign = LabelVerticalalign.Middle
																			}
																		},
																		Orientation = ContainerOrientation.Horizontal
																	},
																	new ContainerReportBlock
																	{
																		BackgroundColor = new ReportItemColor(236, 236, 236),
																		MarginTopPx = 5,
																		MarginLeftPx = 20,
																		PaddingRightPx = 10,
																		ChildProportions = new[] {20, 80},
																		Childs = new IReportBlock[]
																		{
																			new LabelReportBlock
																			{
																				FontStyle = new LabelFontStyle
																				{
																					FontSizePx = 12,
																					FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																				},
																				Text = "Source file:",
																				VerticalAlign = LabelVerticalalign.Middle
																			},
																			new LabelReportBlock
																			{
																				FontStyle = new LabelFontStyle
																				{
																					FontSizePx = 12,
																					FontFamily = "monospace"
																				},
																				Text = "$ReportTaskResultsItem.File$",
																				VerticalAlign = LabelVerticalalign.Middle
																			}
																		},
																		Orientation = ContainerOrientation.Horizontal
																	},
																	new ContainerReportBlock
																	{
																		BackgroundColor = new ReportItemColor(236, 236, 236),
																		MarginTopPx = 5,
																		MarginLeftPx = 20,
																		PaddingRightPx = 10,
																		ChildProportions = new[] {20, 80},
																		Childs = new IReportBlock[]
																		{
																			new LabelReportBlock
																			{
																				FontStyle = new LabelFontStyle
																				{
																					FontSizePx = 12,
																					FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																				},
																				Text = "Query:",
																				VerticalAlign = LabelVerticalalign.Middle
																			},
																			new LabelReportBlock
																			{
																				FontStyle = new LabelFontStyle
																				{
																					FontSizePx = 12,
																					FontFamily = "monospace"
																				},
																				Text = "$ReportTaskResultsItem.Query$",
																				VerticalAlign = LabelVerticalalign.Middle
																			}
																		},
																		Orientation = ContainerOrientation.Horizontal
																	},
																	new ContainerReportBlock
																	{
																		BackgroundColor = new ReportItemColor(236, 236, 236),
																		MarginTopPx = 5,
																		MarginLeftPx = 20,
																		PaddingRightPx = 10,
																		ChildProportions = new[] {20, 80},
																		Childs = new IReportBlock[]
																		{
																			new LabelReportBlock
																			{
																				FontStyle = new LabelFontStyle
																				{
																					FontSizePx = 12,
																					FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																				},
																				Text = "Condition:",
																				VerticalAlign = LabelVerticalalign.Middle
																			},
																			new LabelReportBlock
																			{
																				FontStyle = new LabelFontStyle
																				{
																					FontSizePx = 12,
																					FontFamily = "monospace"
																				},
																				Text = "$ReportTaskResultsItem.Condition$",
																				VerticalAlign = LabelVerticalalign.Middle
																			}
																		},
																		Orientation = ContainerOrientation.Horizontal
																	},
																	new ContainerReportBlock
																	{
																		BackgroundColor = new ReportItemColor(236, 236, 236),
																		MarginTopPx = 5,
																		MarginLeftPx = 20,
																		PaddingRightPx = 10,
																		ChildProportions = new[] {20, 80},
																		Childs = new IReportBlock[]
																		{
																			new LabelReportBlock
																			{
																				FontStyle = new LabelFontStyle
																				{
																					Bold = true,
																					FontSizePx = 12,
																					FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																				},
																				Text = "Issue link:",
																				VerticalAlign = LabelVerticalalign.Middle
																			},
																			new LinkReportBlock("LinkVulnerabilityIssue")
																			{
																				Target = "$ReportTaskResultsItem.IssueUrl$",
																				Child = new LabelReportBlock
																				{
																					Color = new ReportItemColor(0, 158, 226),
																					FontStyle = new LabelFontStyle
																					{
																						Bold = true,
																						FontSizePx = 12,
																						FontFamily = "'Myriad Pro', 'Segoe UI', Arial, Sans-Serif"
																					},
																					Text = "$ReportTaskResultsItem.IssueUrl$",
																					VerticalAlign = LabelVerticalalign.Middle
																				}
																			}
																		},
																		Orientation = ContainerOrientation.Horizontal
																	}
																},
																Orientation = ContainerOrientation.Vertical
															}
														},
														Orientation = ContainerOrientation.Horizontal
													}
												}
											},
											Id = "IteratorVulnerabilities",
											QueryKey = "ReportTaskResults"
										}
									},
									Orientation = ContainerOrientation.Vertical
								}
							},
							Orientation = ContainerOrientation.Vertical
						},
						Id = "HtmlRoot"
					}
				}
			};

			var serializedRule = reportRule.ToJson();

			serializedRule.Should().NotBeNullOrEmpty();
		}
	}
}