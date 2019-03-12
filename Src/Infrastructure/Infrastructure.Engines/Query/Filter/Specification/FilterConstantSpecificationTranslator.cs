namespace Infrastructure.Engines.Query.Filter.Specification
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl.Query.Filter.Specification;

	[UsedImplicitly]
	internal class FilterConstantSpecificationTranslator :
		IFilterSpecificationTranslator<FilterConstantSpecification>
	{
		private readonly IQueryVariableNameBuilder _queryVariableNameBuilder;

		public FilterConstantSpecificationTranslator([NotNull] IQueryVariableNameBuilder queryVariableNameBuilder)
		{
			if (queryVariableNameBuilder == null) throw new ArgumentNullException(nameof(queryVariableNameBuilder));

			_queryVariableNameBuilder = queryVariableNameBuilder;
		}

		public string Translate([NotNull] FilterConstantSpecification specification)
		{
			if (specification == null) throw new ArgumentNullException(nameof(specification));

			return _queryVariableNameBuilder.IsProperty(specification.Value)
				? _queryVariableNameBuilder.ToProperty(specification.Value)
				: specification.Value;
		}

		public string ToDsl([NotNull] FilterConstantSpecification specification)
		{
			if (specification == null)
				throw new ArgumentNullException(nameof(specification));

			return specification.Value;
		}
	}
}