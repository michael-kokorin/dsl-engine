namespace Infrastructure.Query.Tests
{
	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Query;

	[TestFixture]
	public sealed class QueryModelProcessorTests
	{
		private IQueryModelProcessor _target;

		private Mock<IQueryTranslator> _queryTranslator;

		private Mock<IQueryModelAccessValidator> _queryModelAccessValidator;

		[SetUp]
		public void SetUp()
		{
			_queryTranslator = new Mock<IQueryTranslator>();
			_queryModelAccessValidator = new Mock<IQueryModelAccessValidator>();

			_target = new QueryModelProcessor(_queryTranslator.Object, _queryModelAccessValidator.Object);
		}

		[Test]
		public void ShouldConvertQueryFromText()
		{
			const string query = "query";

			const int projectId = 4;

			var queryModel = new DslDataQuery();

			_queryTranslator.Setup(_ => _.ToQuery(query)).Returns(queryModel);

			_queryModelAccessValidator.Setup(_ => _.Validate(queryModel, projectId, false));

			var result = _target.FromText(query, projectId, false);

			result.ShouldBeEquivalentTo(queryModel);
		}

		[Test]
		public void ShouldConvertQueryFromDsl()
		{
			const string query = "query";

			const int projectId = 4;

			var queryModel = new DslDataQuery();

			_queryTranslator.Setup(_ => _.ToDsl(queryModel)).Returns(query);

			_queryModelAccessValidator.Setup(_ => _.Validate(queryModel, projectId, false));

			var result = _target.ToText(queryModel, projectId, false);

			result.ShouldBeEquivalentTo(query);
		}
	}
}