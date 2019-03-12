namespace Infrastructure.Engines.Tests.Query.Filter.Specification
{
	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.Engines.Dsl.Query.Filter.Specification;
	using Infrastructure.Engines.Query;
	using Infrastructure.Engines.Query.Filter.Specification;

	[TestFixture]
	public sealed class FilterConstantSpecificationTranslatorTest
	{
		private FilterConstantSpecificationTranslator _target;

		private Mock<IQueryVariableNameBuilder> _queryVariableNameBuilder;

		[SetUp]
		public void SetUp()
		{
			_queryVariableNameBuilder = new Mock<IQueryVariableNameBuilder>();

			_target = new FilterConstantSpecificationTranslator(_queryVariableNameBuilder.Object);
		}

		[TestCase("1")]
		[TestCase("Halo")]
		[TestCase("#Halo#")]
		public void ShouldTranslateFilterConstraintSpecification(string value)
		{
			var result = _target.Translate(new FilterConstantSpecification
			{
				Value = value
			});

			result.ShouldBeEquivalentTo(value);
		}
	}
}