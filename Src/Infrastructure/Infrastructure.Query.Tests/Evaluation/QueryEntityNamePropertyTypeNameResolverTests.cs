namespace Infrastructure.Query.Tests.Evaluation
{
	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.Engines;
	using Infrastructure.Query.Evaluation;
	using Infrastructure.Query.Evaluation.Exceptions;

	[TestFixture]
	public sealed class QueryEntityNamePropertyTypeNameResolverTests
	{
		private IQueryEntityNamePropertyTypeNameResolver _target;

		private Mock<IQueryEntityNameTranslator> _queryEntityNameTranslator;

		[SetUp]
		public void SetUp()
		{
			_queryEntityNameTranslator = new Mock<IQueryEntityNameTranslator>();

			_target = new QueryEntityNamePropertyTypeNameResolver(_queryEntityNameTranslator.Object);
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public sealed class TestDataSource
		{
			// ReSharper disable once UnusedMember.Global
			public string Prop { get; set; }
		}

		[Test]
		public void ShouldReturnPropertyShortTypeName()
		{
			var type = typeof(TestDataSource);

			_queryEntityNameTranslator.Setup(_ => _.GetEntityType(type.Name)).Returns(type);

			var result = _target.ResolvePropertyTypeName(type.Name, "Prop");

			result.Should().BeEquivalentTo("string");
		}
	}
}