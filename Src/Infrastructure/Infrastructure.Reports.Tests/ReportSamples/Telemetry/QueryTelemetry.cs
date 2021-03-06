﻿namespace Infrastructure.Reports.Tests.ReportSamples.Telemetry
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Reports.Blocks.Container;
	using Infrastructure.Reports.Blocks.HtmlDoc;

	[TestFixture]
	public sealed class QueryTelemetry
	{
		[Test]
		public void ShouldGenerateQueryTelemetryReport()
		{
			const string queryKey = "QueryOperations";

			var reportRule = new ReportRule
			{
				Parameters = new ReportParameter[0],
				QueryLinks =
					new IReportQuery[]
					{
						new ReportQuery
						{
							Key = queryKey,
							Text = @"QueryTelemetry
order #DateTimeLocal# asc
select
DateTimeLocal=#DateTimeLocal.ToString()#
#OperationName#
#OperationDuration#
#UserLogin#
#OperationStatus#
#DisplayName#
#IsSystem#
select end"
						}
					},
				ReportTitle = "Query telemetry",
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
								ProjectTelemetryReport.GetOperationDurationChart(queryKey),
								VcsTelemetryReport.GetTable(queryKey, "Table 1. Query operations")
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