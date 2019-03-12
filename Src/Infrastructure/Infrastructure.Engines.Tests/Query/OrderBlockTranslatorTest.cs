namespace Infrastructure.Engines.Tests.Query
{
	using System.Data.SqlClient;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.Engines.Dsl;
	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Query;

	[TestFixture]
	public sealed class OrderBlockTranslatorTest
	{
		private OrderBlockTranslator _target;

		private Mock<IQueryVariableNameBuilder> _queryVariableNameBuilder;

		[SetUp]
		public void SetUp()
		{
			_queryVariableNameBuilder = new Mock<IQueryVariableNameBuilder>();

			_queryVariableNameBuilder
				.Setup(_ => _.Encode(It.IsAny<string>()))
				.Returns<string>(_ => $"#{_}#");

			_target = new OrderBlockTranslator(_queryVariableNameBuilder.Object);
		}

		private static readonly object[] TestCases =
		{
			new object[]
			{
				new[]
				{
					new OrderBlockItem {OrderFieldName = "a", SortOrder = SortOrder.Ascending}
				},
				".OrderBy(x => x.a)\r\n"
			},
			new object[]
			{
				new[]
				{
					new OrderBlockItem {OrderFieldName = "a", SortOrder = SortOrder.Ascending},
					new OrderBlockItem {OrderFieldName = "b", SortOrder = SortOrder.Descending}
				},
				".OrderBy(x => x.a)\r\n.ThenByDescending(x => x.b)\r\n"
			}
		};

		[Test]
		[TestCaseSource(nameof(TestCases))]
		public void ShouldTranslateOrderBlock(OrderBlockItem[] orderItems, string value)
		{
			var result = _target.Translate(
				new DslOrderBlock
				{
					Items = orderItems
				});

			result.ShouldBeEquivalentTo(value);
		}

		private static readonly object[] TestCasesDsl =
		{
			new object[]
			{
				new[]
				{
					new OrderBlockItem {OrderFieldName = "a", SortOrder = SortOrder.Ascending}
				},
				"order #a# asc"
			},
			new object[]
			{
				new[]
				{
					new OrderBlockItem {OrderFieldName = "a", SortOrder = SortOrder.Ascending},
					new OrderBlockItem {OrderFieldName = "b", SortOrder = SortOrder.Descending}
				},
				"order #a# asc, #b# desc"
			}
		};

		[Test]
		[TestCaseSource(nameof(TestCasesDsl))]
		public void ShouldTranslateOrderBlockToDsl(OrderBlockItem[] orderItems, string value)
		{
			var result = _target.ToDsl(
				new DslOrderBlock
				{
					Items = orderItems
				});

			result.ShouldBeEquivalentTo(value);
		}
	}
}