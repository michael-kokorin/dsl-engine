namespace Infrastructure.Engines.Dsl.Tests.Query
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Engines.Dsl.Query;

	[TestFixture]
	public sealed class DslGroupBlockTest
	{
		[Test]
		public void Test()
		{
			var groupBlock = new DslGroupBlock
			{
				Items = new[]
				{
					new DslGroupItem
					{
						VariableName = "Halo"
					}
				}
			};

			var json = groupBlock.ToJson();

			json.Should().NotBeNullOrEmpty();

			var deserialized = json.FromJson<DslGroupBlock>();

			deserialized.Items.Should().NotBeNullOrEmpty();
		}
	}
}