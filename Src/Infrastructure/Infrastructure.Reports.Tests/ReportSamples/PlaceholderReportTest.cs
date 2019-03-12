namespace Infrastructure.Reports.Tests.ReportSamples
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Reports.Blocks.Label;

	[TestFixture]
	public sealed class PlaceholderReportTest
	{
		[Test]
		public void ShouldCreatePlaceholderReport()
		{
			var reportRule = new ReportRule
			{
				Parameters = new ReportParameter[0],
				QueryLinks = new IReportQuery[0],
				ReportTitle = "AI SSDL analyst report",
				Template = new ReportTemplate
				{
					Root = new LabelReportBlock
					{
						Id = "PlaceholderLabel",
						Text = "Placehoder for some data."
					}
				}
			};

			var serializedRule = reportRule.ToJson();

			serializedRule.Should().NotBeNullOrEmpty();
		}
	}
}