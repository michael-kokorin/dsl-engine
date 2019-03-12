namespace Infrastructure.Reports.Tests.Blocks.Label
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Reports.Blocks.Label;

	[TestFixture]
	public sealed class LabelReportBlockTest
	{
		[Test]
		public void LabelShouldBeXmlSerializable()
		{
			var label = new LabelReportBlock();

			var serialized = label.ToXml();

			var deserialized = serialized.FromXml<LabelReportBlock>();

			label.ShouldBeEquivalentTo(deserialized);
		}
	}
}