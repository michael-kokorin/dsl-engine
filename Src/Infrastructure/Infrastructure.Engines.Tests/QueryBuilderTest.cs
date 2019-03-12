namespace Infrastructure.Engines.Tests
{
	using System;
	using System.Linq;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Data;
	using Infrastructure.Engines.Dsl;
	using Infrastructure.Engines.Dsl.Query;

	[TestFixture]
	public sealed class QueryBuilderTest
	{
		private IQueryBuilder _target;

		private Mock<IDataQueryExecutor> _dataQueryExecutor;

		private Mock<IDslParser> _dslParser;

		private Mock<IDataQueryExpressionTranslator> _dataQueryExpressionTranslator;

		private Mock<IQueryEntityNameTranslator> _queryEntityNameTranslator;

		private Mock<IDataSourceProvider> _dataSourceProvider;

		[SetUp]
		public void SetUp()
		{
			_dataQueryExecutor = new Mock<IDataQueryExecutor>();

			_dslParser = new Mock<IDslParser>();

			_dataQueryExpressionTranslator = new Mock<IDataQueryExpressionTranslator>();

			_queryEntityNameTranslator = new Mock<IQueryEntityNameTranslator>();

			_dataSourceProvider = new Mock<IDataSourceProvider>();

			_target = new QueryBuilder(_dataQueryExecutor.Object,
				_dslParser.Object,
				_dataQueryExpressionTranslator.Object,
				_queryEntityNameTranslator.Object,
				_dataSourceProvider.Object);
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public sealed class TestDataSource<T> : IDataSource<T>
		{
			/// <summary>
			///     Queries data.
			/// </summary>
			/// <returns>Data.</returns>
			public IQueryable<T> Query()
			{
				throw new NotImplementedException();
			}

			/// <summary>
			///   Queries data.
			/// </summary>
			/// <returns>Data.</returns>
			IQueryable<object> IDataSource.Query() => (IQueryable<object>)Query();
		}

		[Test]
		public void ShouldProcessQuery()
		{
			const string textQuery = "query";

			const string entityName = "entityName";

			var query = new DslDataQuery
			{
				QueryEntityName = entityName
			};

			const string expr = "expr";

			var dataSource = new TestDataSource<object>();

			var resultObject = new object();

			_dslParser.Setup(_ => _.DataQueryParse(textQuery)).Returns(query);

			_dataQueryExpressionTranslator.Setup(_ => _.Translate(query)).Returns(expr);

			_queryEntityNameTranslator.Setup(_ => _.GetEntityType(entityName)).Returns(GetType);

			_dataSourceProvider.Setup(_ => _.GetDataSource(GetType())).Returns(dataSource);

			_dataQueryExecutor.Setup(_ => _.Execute(dataSource, GetType(), expr)).Returns(resultObject);

			var result = _target.Execute(textQuery);

			_dslParser.Verify(_ => _.DataQueryParse(textQuery), Times.Once);

			_dataQueryExpressionTranslator.Verify(_ => _.Translate(query), Times.Once);

			_queryEntityNameTranslator.Verify(_ => _.GetEntityType(entityName), Times.Once);

			_dataSourceProvider.Verify(_ => _.GetDataSource(GetType()), Times.Once);

			_dataQueryExecutor.Verify(_ => _.Execute(dataSource, GetType(), expr), Times.Once);

			result.ShouldBeEquivalentTo(resultObject);
		}
	}
}