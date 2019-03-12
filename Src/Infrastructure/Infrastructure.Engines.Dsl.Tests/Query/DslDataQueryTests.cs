namespace Infrastructure.Engines.Dsl.Tests.Query
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Engines.Dsl.Query;

	[TestFixture]
	public sealed class DslDataQueryTests
	{
		[Test]
		public void ShouldSetializeDataQueryToJson()
		{
			var dataQuery = new DslDataQuery
			{
				Blocks = new IDslQueryBlock[]
				{
					new DslGroupBlock()
				}
			};

			var result = dataQuery.ToJson();

			result.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void ShouldDeserializeDataQueryFromJson()
		{
			var dataQuery = new DslDataQuery
			{
				Blocks = new IDslQueryBlock[]
				{
					new DslLimitBlock()
				},
				QueryEntityName = "testQuery"
			};

			var json = dataQuery.ToJson();

			var result = json.FromJson<DslDataQuery>();

			result.Should().NotBeNull();

			result.QueryEntityName.ShouldBeEquivalentTo(dataQuery.QueryEntityName);
		}
	}
}