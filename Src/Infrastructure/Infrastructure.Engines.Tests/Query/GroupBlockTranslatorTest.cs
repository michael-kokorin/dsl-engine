namespace Infrastructure.Engines.Tests.Query
{
	using System.Linq;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Query;

	[TestFixture]
	public sealed class GroupBlockTranslatorTest
	{
		private IQueryBlockTranslator<DslGroupBlock> _target;

		private Mock<IQueryVariableNameBuilder> _queryVariableNameBuilder;

		[SetUp]
		public void SetUp()
		{
			_queryVariableNameBuilder = new Mock<IQueryVariableNameBuilder>();

			_queryVariableNameBuilder
				.Setup(_ => _.ToProperty(It.IsAny<string>()))
				.Returns<string>(_ => $"x.{_}");

			_queryVariableNameBuilder
				.Setup(_ => _.Encode(It.IsAny<string>()))
				.Returns<string>(_ => $"#{_}#");

			_target = new GroupBlockTranslator(_queryVariableNameBuilder.Object);
		}

		private static readonly object[] TestCases =
		{
			new object[]
			{
				new string[] {},
				".GroupBy(x => new{})"
			},
			new object[]
			{
				new[] {"a"},
				".GroupBy(x => new{x.#a#})"
			},
			new object[]
			{
				new[] {"a", "b"},
				".GroupBy(x => new{x.#a#,x.#b#})"
			}
		};

		[TestCaseSource(nameof(TestCases))]
		public void ShouldTranslateQueryGroupBlock(string[] groupItems, string available)
		{
			var result = _target.Translate(new DslGroupBlock
			{
				Items = groupItems.Select(_ => new DslGroupItem
				{
					VariableName = _
				})
			});

			result.Should().BeEquivalentTo(available);
		}

		private static readonly object[] DslTestCases =
		{
			new object[]
			{
				new string[] {},
				"group "
			},
			new object[]
			{
				new[] {"a"},
				"group #a#"
			},
			new object[]
			{
				new[] {"a", "b"},
				"group #a#,#b#"
			}
		};

		[TestCaseSource(nameof(DslTestCases))]
		public void ShouldTranslateGroupBlockToDsl(string[] groupItems, string available)
		{
			var result = _target.ToDsl(new DslGroupBlock
			{
				Items = groupItems.Select(_ => new DslGroupItem
				{
					VariableName = _
				})
			});

			result.Should().BeEquivalentTo(available);
		}
	}
}