namespace Infrastructure.Reports.Tests.ReportSamples
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Reports.Blocks.Iterator;
	using Infrastructure.Reports.Blocks.QueryScope;
	using Infrastructure.Reports.Blocks.Table;

	[TestFixture]
	public sealed class InnerQueriesExample
	{
		[Test]
		public void ShouldGenerateExampleReportWithInnerQueries()
		{
			var reportRule = new ReportRule
			{
				Parameters = new ReportParameter[0],
				QueryLinks = new IReportQuery[]
				{
					new ReportQuery
					{
						Key = "Tasks",
						Text = @"Tasks
select
#Id#
#Repository#
select end"
					}
				},
				ReportTitle = "Test query",
				Template = new ReportTemplate
				{
					Root = new IteratorReportBlock
					{
						Child = new QueryScopeReportBlock
						{
							Query = new ReportQuery
							{
								Key = "Results",
								Text = @"TaskResults
where #TaskId# == {Id}
select
#Id#
#Type#
#RawLine#
select end"
							},
							Child = new TableReportBlock
							{
								BorderPx = 1,
								QueryKey = "Results"
							},
							Parameters = new []
							{
								new QueryScopeParameter
								{
									Key = "Id",
									Template = "$TasksItem.Id$"
								}
							}
						},
						QueryKey = "Tasks"
					}
				}
			};

			var serializedRule = reportRule.ToJson();

			serializedRule.Should().NotBeNullOrEmpty();
		}
	}
}