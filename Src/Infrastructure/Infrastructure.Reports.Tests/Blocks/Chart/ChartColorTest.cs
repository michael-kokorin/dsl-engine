namespace Infrastructure.Reports.Tests.Blocks.Chart
{
	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Reports.Blocks;

	[TestFixture]
	public sealed class ChartColorTest
	{
		[Test]
		public void ShouldConvertColorToString()
		{
			var color = new ReportItemColor(10,20,30, 0.2f);

			var result = color.ToRgba();

			result.ShouldBeEquivalentTo("rgba(10, 20, 30, 0.2)");
		}
	}
}