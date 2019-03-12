namespace Infrastructure.Engines.Query.Filter.Specification
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl.Query.Filter.Specification;

	[UsedImplicitly]
	internal sealed class FilterArraySpecificationTranslator : IFilterSpecificationTranslator<FilterArraySpecification>
	{
		private readonly IFilterSpecificationTranslatorDirector _filterSpecificationTranslatorDirector;

		public FilterArraySpecificationTranslator(
			[NotNull] IFilterSpecificationTranslatorDirector filterSpecificationTranslatorDirector)
		{
			if (filterSpecificationTranslatorDirector == null)
				throw new ArgumentNullException(nameof(filterSpecificationTranslatorDirector));

			_filterSpecificationTranslatorDirector = filterSpecificationTranslatorDirector;
		}

		public string Translate([NotNull] FilterArraySpecification specification)
		{
			if (specification == null) throw new ArgumentNullException(nameof(specification));

			var translatedSpecifications = specification
				.Specifications
				.Select(_ => _filterSpecificationTranslatorDirector.Translate((dynamic) _));

			return $"new[]{{{string.Join(",", translatedSpecifications)}}}";
		}

		public string ToDsl([NotNull] FilterArraySpecification specification)
		{
			if (specification == null) throw new ArgumentNullException(nameof(specification));

			var translatedSpecifications = specification
				.Specifications
				.Select(_ => _filterSpecificationTranslatorDirector.ToDsl((dynamic) _));

			return $"({string.Join(",", translatedSpecifications)})";
		}
	}
}