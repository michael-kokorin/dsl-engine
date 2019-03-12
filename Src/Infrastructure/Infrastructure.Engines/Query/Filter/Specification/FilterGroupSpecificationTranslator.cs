namespace Infrastructure.Engines.Query.Filter.Specification
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl.Query.Filter.Specification;

	[UsedImplicitly]
	internal sealed class FilterGroupSpecificationTranslator : IFilterSpecificationTranslator<FilterGroupSpecification>
	{
		private readonly IFilterSpecificationTranslatorDirector _filterSpecificationTranslatorDirector;

		public FilterGroupSpecificationTranslator(
			[NotNull] IFilterSpecificationTranslatorDirector filterSpecificationTranslatorDirector)
		{
			if (filterSpecificationTranslatorDirector == null)
				throw new ArgumentNullException(nameof(filterSpecificationTranslatorDirector));

			_filterSpecificationTranslatorDirector = filterSpecificationTranslatorDirector;
		}

		public string Translate([NotNull] FilterGroupSpecification specification)
		{
			if (specification == null) throw new ArgumentNullException(nameof(specification));

			var innerSpecification = _filterSpecificationTranslatorDirector.Translate((dynamic) specification.Specification);

			return $"({innerSpecification})";
		}

		public string ToDsl([NotNull] FilterGroupSpecification specification)
		{
			if (specification == null) throw new ArgumentNullException(nameof(specification));

			var innerSpecification = _filterSpecificationTranslatorDirector.ToDsl((dynamic) specification.Specification);

			return $"({innerSpecification})";
		}
	}
}