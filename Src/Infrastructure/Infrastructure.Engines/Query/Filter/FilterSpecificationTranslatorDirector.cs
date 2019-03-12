namespace Infrastructure.Engines.Query.Filter
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl.Query.Filter.Specification;

	[UsedImplicitly]
	internal sealed class FilterSpecificationTranslatorDirector : IFilterSpecificationTranslatorDirector
	{
		private readonly IFilterSpecificationTranslatorResolver _filterSpecificationTranslatorResolver;

		public FilterSpecificationTranslatorDirector(
			[NotNull] IFilterSpecificationTranslatorResolver filterSpecificationTranslatorResolver)
		{
			if (filterSpecificationTranslatorResolver == null)
				throw new ArgumentNullException(nameof(filterSpecificationTranslatorResolver));

			_filterSpecificationTranslatorResolver = filterSpecificationTranslatorResolver;
		}

		public string Translate<T>([NotNull] T specification) where T : class, IFilterSpecification
		{
			if (specification == null) throw new ArgumentNullException(nameof(specification));

			var translator = GetTranslator((dynamic) specification);

			return translator.Translate(specification);
		}

		private IFilterSpecificationTranslator<T> GetTranslator<T>(T specification) where T : class, IFilterSpecification =>
			_filterSpecificationTranslatorResolver.Resolve((dynamic) specification);

		public string ToDsl<T>([NotNull] T specification) where T : class, IFilterSpecification
		{
			if (specification == null) throw new ArgumentNullException(nameof(specification));

			var translator = GetTranslator((dynamic) specification);

			return translator.ToDsl(specification);
		}
	}
}