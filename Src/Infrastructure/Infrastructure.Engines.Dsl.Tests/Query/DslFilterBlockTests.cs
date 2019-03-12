namespace Infrastructure.Engines.Dsl.Tests.Query
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Engines.Dsl.Query.Filter;

	[TestFixture]
	public sealed class DslFilterBlockTests
	{
		[Test]
		public void ShouldSerializeDslFilterBlock()
		{
			const string expression = "1 < 2 && 3 == 3 && 5 > 4";

			var filterBlock = DslFilterBlockParser.ParseQuery(expression);

			var serialized = filterBlock.ToJson();

			var deserializedBlock = serialized.FromJson<DslFilterBlock>();

			deserializedBlock.Should().NotBeNull();
		}
	}
}