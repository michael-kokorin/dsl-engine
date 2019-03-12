namespace Infrastructure.Engines.Tests.Query
{
	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.Engines.Dsl;
	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Query;

	[TestFixture]
	public sealed class QueryTranslatorTests
	{
		private IQueryTranslator _target;

		private Mock<IDslParser> _dslParser;

		private Mock<IQueryBlockTranslationManager> _queryBlockTranslationManager;

		[SetUp]
		public void SetUp()
		{
			_dslParser = new Mock<IDslParser>();

			_queryBlockTranslationManager = new Mock<IQueryBlockTranslationManager>();

			_target = new QueryTranslator(_dslParser.Object, _queryBlockTranslationManager.Object);
		}

		[Test]
		public void ShouldConvertQueryToModel()
		{
			const string query = "query";

			var queryModel = new DslDataQuery();

			_dslParser.Setup(_ => _.DataQueryParse(query)).Returns(queryModel);

			var result = _target.ToQuery(query);

			result.ShouldBeEquivalentTo(queryModel);
		}

		[Test]
		public void ShouldConvertQueryToDsl()
		{
			var queryModel = new DslDataQuery
			{
				Blocks = new []
				{
					new DslLimitBlock
					{
						Skip = 1,
						Take = 2
					}
				},
				QueryEntityName = "entityName",
				TableKey = "table",
				TakeFirst = true,
				TakeFirstOrDefault = true
			};

			var result = _target.ToDsl(queryModel);

			result.Should().NotBeNullOrEmpty();
		}
	}
}