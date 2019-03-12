namespace Infrastructure.Query.Tests
{
	using System.Collections.Generic;
	using System.Linq;

	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Engines.Dsl.Query;

	[TestFixture]
	internal sealed class QueryModelValidatorTest
	{
		private IQueryModelValidator _target;

		[SetUp]
		public void SetUp() => _target = new QueryModelValidator();

		[Test]
		public void ShouldRemoveEmptyFormatItems()
		{
			var formatBlock = new DslFormatBlock
			{
				Selects = new List<DslFormatItem>
				{
					new DslFormatItem
					{
						Value = "Test"
					},
					new DslFormatItem
					{
						Value = string.Empty
					},
					new DslFormatItem
					{
						Value = null
					}
				}
			};

			var sourceModel = new DslDataQuery
			{
				Blocks = new IDslQueryBlock[]
				{
					formatBlock
				}
			};

			_target.Validate(sourceModel);

			formatBlock.Selects.Count().ShouldBeEquivalentTo(1);
		}
	}
}