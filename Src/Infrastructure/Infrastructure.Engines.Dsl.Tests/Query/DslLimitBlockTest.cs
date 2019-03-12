namespace Infrastructure.Engines.Dsl.Tests.Query
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Engines.Dsl.Query;

	[TestFixture]
	public sealed class DslLimitBlockTest
	{
		[Test]
		public void ShouldSerializeDslLimitBlock()
		{
			var limitBlock = new DslLimitBlock
			{
				Skip = 10,
				Take = 4
			};

			var json = limitBlock.ToJson();

			var deserialized = json.FromJson<DslLimitBlock>();

			deserialized.Skip.ShouldBeEquivalentTo(limitBlock.Skip);
			deserialized.Take.ShouldBeEquivalentTo(limitBlock.Take);
		}
	}
}