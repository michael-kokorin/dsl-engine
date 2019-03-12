namespace Infrastructure.Reports.Tests.ReportSamples
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Extensions;
	using Common.FileSystem;
	using Infrastructure.Reports.Blocks.Html;

	[TestFixture]
	public sealed class NotificationHtmlReport
	{
		[Test]
		public void GenerateDevScanFinishedMailReport()
		{
			var body = FileLoader.FromResource($"{GetType().Namespace}.NotificationTemplates.DevScanFinished.html");

			var repo = new ReportRule
			{
				Parameters = new[]
				{
					new ReportParameter("Task Id", "TaskId", "1")
				},
				QueryLinks = new IReportQuery[]
				{
					new ReportQuery
					{
						Key = "Task",
						Text = @"Tasks
where #Id# == {TaskId}
select
#Id#
#ProjectId#
#Created#
#Finished#
Branch=#Repository#
#SdlStatus#
ProjectName=#Projects.DisplayName#
ProjectSdlStatus=#Projects.SdlPolicyStatus#
ProjectBranch=#Projects.DefaultBranchName#
TaskSdlAccomplished=#SdlStatus# == 1
ProjectSdlAccomplished=#Projects.SdlPolicyStatus# == 1
HasVulnerabilities=#Todo# + #Reopen# > 0
#HighSeverityVulns#
TotalVulnerabilities=#HighSeverityVulns#+#LowSeverityVulns#+#MediumSeverityVulns#
select end"
					},
					new ReportQuery
					{
						Key = "Results",
						Text = @"TaskResults
where #TaskId# == {TaskId}
select
#Id#
#File#
#SeverityType#
#IssueNumber#
#IssueUrl#
#Type#
#LineNumber#
IsHighSeverity=#SeverityType# == 3
IsMedSeverity=#SeverityType# == 2
IsLowSeverity=#SeverityType# == 1
select end
order #SeverityType# desc, #Type# asc, #File# asc"
					}
				},
				ReportTitle =
					"$if(Task)$ $Task:{ Task | [Dev] FYA: $Task.Value.ProjectName$ - Scan finished (branch: $Task.Value.Branch$) }$ $endif$",
				Template = new ReportTemplate
				{
					Root = new HtmlReportBlock
					{
						Id = "HtmlBody",
						Template = body
					}
				}
			};

			var serialized = repo.ToJson();

			serialized.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void GenerateDevPolicySucceedMailReport()
		{
			var body = FileLoader.FromResource($"{GetType().Namespace}.NotificationTemplates.DevPolicySucceed.html");

			var repo = new ReportRule
			{
				Parameters = new[]
				{
					new ReportParameter("Task Id", "TaskId", "1")
				},
				QueryLinks = new IReportQuery[]
				{
					new ReportQuery
					{
						Key = "Task",
						Text = @"Tasks
where #Id# == {TaskId}
select
#Id#
#ProjectId#
#Created#
#Finished#
Branch=#Repository#
#SdlStatus#
ProjectName=#Projects.DisplayName#
ProjectSdlStatus=#Projects.SdlPolicyStatus#
ProjectBranch=#Projects.DefaultBranchName#
TaskSdlAccomplished=#SdlStatus# == 1
ProjectSdlAccomplished=#Projects.SdlPolicyStatus# == 1
HasVulnerabilities=#Todo# + #Reopen# > 0
#HighSeverityVulns#
TotalVulnerabilities=#HighSeverityVulns#+#LowSeverityVulns#+#MediumSeverityVulns#
select end"
					}
				},
				ReportTitle =
					"$if(Task)$ $Task:{ Task | [Dev] FYI: $Task.Value.ProjectName$ - Policy accomplished (branch: $Task.Value.ProjectBranch$) }$ $endif$",
				Template = new ReportTemplate
				{
					Root = new HtmlReportBlock
					{
						Id = "HtmlBody",
						Template = body
					}
				}
			};

			var serialized = repo.ToJson();

			serialized.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void GenerateDevPolicyViolatedMailReport()
		{
			var body = FileLoader.FromResource($"{GetType().Namespace}.NotificationTemplates.DevPolicyViolated.html");

			var repo = new ReportRule
			{
				Parameters = new[]
				{
					new ReportParameter("Task Id", "TaskId", "1")
				},
				QueryLinks = new IReportQuery[]
				{
					new ReportQuery
					{
						Key = "Task",
						Text = @"Tasks
where #Id# == {TaskId}
select
#Id#
#ProjectId#
#Created#
#Finished#
Branch=#Repository#
#SdlStatus#
ProjectName=#Projects.DisplayName#
ProjectSdlStatus=#Projects.SdlPolicyStatus#
ProjectBranch=#Projects.DefaultBranchName#
TaskSdlAccomplished=#SdlStatus# == 1
ProjectSdlAccomplished=#Projects.SdlPolicyStatus# == 1
HasVulnerabilities=#Todo# + #Reopen# > 0
#HighSeverityVulns#
TotalVulnerabilities=#HighSeverityVulns#+#LowSeverityVulns#+#MediumSeverityVulns#
select end"
					},
					new ReportQuery
					{
						Key = "Results",
						Text = @"TaskResults
where #TaskId# == {TaskId}
select
#Id#
#File#
#SeverityType#
#IssueNumber#
#IssueUrl#
#Type#
#LineNumber#
IsHighSeverity=#SeverityType# == 3
IsMedSeverity=#SeverityType# == 2
IsLowSeverity=#SeverityType# == 1
select end
order #SeverityType# desc, #Type# asc, #File# asc"
					}
				},
				ReportTitle =
					"$if(Task)$ $Task:{ Task | [Dev] FYA: $Task.Value.ProjectName$ - Policy violated (branch: $Task.Value.ProjectBranch$) }$ $endif$",
				Template = new ReportTemplate
				{
					Root = new HtmlReportBlock
					{
						Id = "HtmlBody",
						Template = body
					}
				}
			};

			var serialized = repo.ToJson();

			serialized.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void GenerateMgrPolicySucceedMailReport()
		{
			var body = FileLoader.FromResource($"{GetType().Namespace}.NotificationTemplates.ManPolicySucceed.html");

			var repo = new ReportRule
			{
				Parameters = new[]
				{
					new ReportParameter("Task Id", "TaskId", "1")
				},
				QueryLinks = new IReportQuery[]
				{
					new ReportQuery
					{
						Key = "Task",
						Text = @"Tasks
where #Id# == {TaskId}
select
#Id#
#ProjectId#
#Created#
#Finished#
Branch=#Repository#
#SdlStatus#
ProjectName=#Projects.DisplayName#
ProjectSdlStatus=#Projects.SdlPolicyStatus#
ProjectBranch=#Projects.DefaultBranchName#
TaskSdlAccomplished=#SdlStatus# == 1
ProjectSdlAccomplished=#Projects.SdlPolicyStatus# == 1
HasVulnerabilities=#Todo# + #Reopen# > 0
#HighSeverityVulns#
TotalVulnerabilities=#HighSeverityVulns#+#LowSeverityVulns#+#MediumSeverityVulns#
select end"
					}
				},
				ReportTitle =
					"$if(Task)$ $Task:{ Task | [Mgr] FYI: $Task.Value.ProjectName$ - Policy accomplished (branch: $Task.Value.ProjectBranch$) }$ $endif$",
				Template = new ReportTemplate
				{
					Root = new HtmlReportBlock
					{
						Id = "HtmlBody",
						Template = body
					}
				}
			};

			var serialized = repo.ToJson();

			serialized.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void GenerateMgrPolicyViolatedMailReport()
		{
			var body = FileLoader.FromResource($"{GetType().Namespace}.NotificationTemplates.ManPolicyViolated.html");

			var repo = new ReportRule
			{
				Parameters = new[]
				{
					new ReportParameter("Task Id", "TaskId", "1")
				},
				QueryLinks = new IReportQuery[]
				{
					new ReportQuery
					{
						Key = "Task",
						Text = @"Tasks
where #Id# == {TaskId}
select
#Id#
#ProjectId#
#Created#
#Finished#
Branch=#Repository#
#SdlStatus#
ProjectName=#Projects.DisplayName#
ProjectSdlStatus=#Projects.SdlPolicyStatus#
ProjectBranch=#Projects.DefaultBranchName#
TaskSdlAccomplished=#SdlStatus# == 1
ProjectSdlAccomplished=#Projects.SdlPolicyStatus# == 1
HasVulnerabilities=#Todo# + #Reopen# > 0
#HighSeverityVulns#
TotalVulnerabilities=#HighSeverityVulns#+#LowSeverityVulns#+#MediumSeverityVulns#
select end"
					},
					new ReportQuery
					{
						Key = "ResultSummary",
						Text = @"TaskResults
where #TaskId# == {TaskId}
select
#Type#
#SeverityType#
#IssueNumber#
#IssueUrl#
select end
group Type,SeverityType,IssueNumber,IssueUrl
select
Type=#Key.Type#
SeverityType=#Key.SeverityType#
Count=#Count()#
IsLowSeverity=#Key.SeverityType# == 1
IsMedSeverity=#Key.SeverityType# == 2
IsHighSeverity=#Key.SeverityType# == 3
IssueNumber=#Key.IssueNumber#
IssueUrl=#Key.IssueUrl#
select end
order #SeverityType# desc, #Type# asc"
					}
				},
				ReportTitle =
					"$if(Task)$ $Task:{ Task | [Mgr] FYA: $Task.Value.ProjectName$ - Policy violated (branch: $Task.Value.ProjectBranch$) }$ $endif$",

				Template = new ReportTemplate
				{
					Root = new HtmlReportBlock
					{
						Id = "HtmlBody",
						Template = body
					}
				}
			};

			var serialized = repo.ToJson();

			serialized.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void GenerateMgrScanFinishedMailReport()
		{
			var body = FileLoader.FromResource($"{GetType().Namespace}.NotificationTemplates.ManScanFinished.html");

			var repo = new ReportRule
			{
				Parameters = new[]
				{
					new ReportParameter("Task Id", "TaskId", "1")
				},
				QueryLinks = new IReportQuery[]
				{
					new ReportQuery
					{
						Key = "Task",
						Text = @"Tasks
where #Id# == {TaskId}
select
#Id#
#ProjectId#
#Created#
#Finished#
Branch=#Repository#
#SdlStatus#
ProjectName=#Projects.DisplayName#
ProjectSdlStatus=#Projects.SdlPolicyStatus#
ProjectBranch=#Projects.DefaultBranchName#
TaskSdlAccomplished=#SdlStatus# == 1
ProjectSdlAccomplished=#Projects.SdlPolicyStatus# == 1
HasVulnerabilities=#Todo# + #Reopen# > 0
#HighSeverityVulns#
TotalVulnerabilities=#HighSeverityVulns#+#LowSeverityVulns#+#MediumSeverityVulns#
select end"
					},
					new ReportQuery
					{
						Key = "ResultSummary",
						Text = @"TaskResults
where #TaskId# == {TaskId}
select
#Type#
#SeverityType#
#IssueNumber#
#IssueUrl#
select end
group Type,SeverityType,IssueNumber,IssueUrl
select
Type=#Key.Type#
SeverityType=#Key.SeverityType#
Count=#Count()#
IsLowSeverity=#Key.SeverityType# == 1
IsMedSeverity=#Key.SeverityType# == 2
IsHighSeverity=#Key.SeverityType# == 3
IssueNumber=#Key.IssueNumber#
IssueUrl=#Key.IssueUrl#
select end
order #SeverityType# desc, #Type# asc"
					}
				},
				ReportTitle =
					"$if(Task)$ $Task:{ Task | [Man] FYA: $Task.Value.ProjectName$ - Scan finished (branch: $Task.Value.Branch$) }$ $endif$",

				Template = new ReportTemplate
				{
					Root = new HtmlReportBlock
					{
						Id = "HtmlBody",
						Template = body
					}
				}
			};

			var serialized = repo.ToJson();

			serialized.Should().NotBeNullOrEmpty();
		}
	}
}