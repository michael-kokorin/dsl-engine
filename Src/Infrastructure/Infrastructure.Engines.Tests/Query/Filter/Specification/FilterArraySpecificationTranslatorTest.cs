namespace Infrastructure.Engines.Tests.Query.Filter.Specification
{
	using System.Linq;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.Engines.Dsl.Query.Filter.Specification;
	using Infrastructure.Engines.Query.Filter;
	using Infrastructure.Engines.Query.Filter.Specification;

	[TestFixture]
	public sealed class FilterArraySpecificationTranslatorTest
	{
		private FilterArraySpecificationTranslator _target;

		private Mock<IFilterSpecificationTranslatorDirector> _filterSpecificationTranslatorDirector;

		[SetUp]
		public void SetUp()
		{
			_filterSpecificationTranslatorDirector = new Mock<IFilterSpecificationTranslatorDirector>();

			_filterSpecificationTranslatorDirector
				.Setup(_ => _.Translate(It.IsAny<FilterConstantSpecification>()))
				.Returns<FilterConstantSpecification>(_ => _.Value);

			_target = new FilterArraySpecificationTranslator(_filterSpecificationTranslatorDirector.Object);
		}

		private static readonly object[] TestCases =
		{
			new object[]
			{
				new[] {1, 2, 3},
				"new[]{1,2,3}"
			}
		};

		[TestCaseSource(nameof(TestCases))]
		public void ShouldTranslateArraySpecification(int[] specifications, string available)
		{
			var result = _target.Translate(new FilterArraySpecification
			{
				Specifications= specifications.Select(_ => new FilterConstantSpecification
				{
					Value = _.ToString()
				})
			});

			result.ShouldBeEquivalentTo(available);

			var d = new[] {1, 2, 3}.Contains(2);
		}
	}
}