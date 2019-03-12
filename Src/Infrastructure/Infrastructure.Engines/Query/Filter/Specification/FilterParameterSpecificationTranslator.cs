namespace Infrastructure.Engines.Query.Filter.Specification
{
	using JetBrains.Annotations;

	internal sealed class FilterParameterSpecificationTranslator : FilterConstantSpecificationTranslator
	{
		public FilterParameterSpecificationTranslator([NotNull] IQueryVariableNameBuilder queryVariableNameBuilder)
			: base(queryVariableNameBuilder)
		{
		}
	}
}