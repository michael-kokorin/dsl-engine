namespace Infrastructure.Engines.Query
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl;
	using Infrastructure.Engines.Dsl.Query.Filter;
	using Infrastructure.Engines.Query.Filter;

	[UsedImplicitly]
	internal sealed class FilterBlockTranslator : IQueryBlockTranslator<DslFilterBlock>
	{
		private readonly IFilterSpecificationTranslatorDirector _filterSpecificationTranslatorDirector;

		public FilterBlockTranslator(
			[NotNull] IFilterSpecificationTranslatorDirector filterSpecificationTranslatorDirector)
		{
			if (filterSpecificationTranslatorDirector == null)
				throw new ArgumentNullException(nameof(filterSpecificationTranslatorDirector));

			_filterSpecificationTranslatorDirector = filterSpecificationTranslatorDirector;
		}

		public string Translate([NotNull] DslFilterBlock queryBlock)
		{
			if (queryBlock == null) throw new ArgumentNullException(nameof(queryBlock));

			var translated = _filterSpecificationTranslatorDirector.Translate((dynamic) queryBlock.Specification);

			return $".Where(x => {translated})";
		}

		public string ToDsl([NotNull] DslFilterBlock queryBlock)
		{
			if (queryBlock == null) throw new ArgumentNullException(nameof(queryBlock));

			var specification = _filterSpecificationTranslatorDirector.ToDsl((dynamic) queryBlock.Specification);

			return $"{DslKeywords.Where} {specification}";
		}
	}
}