namespace Infrastructure.Engines.Dsl.Query.Filter.Specification
{
	using System.Collections.Generic;
	using System.Linq;

	public sealed class FilterSpecification : IFilterSpecification
	{
		public IFilterSpecification LeftSpecification { get; set; }

		public FilterOperator Operator { get; set; }

		public IFilterSpecification RightSpecification { get; set; }

		public IEnumerable<DslQueryParameter> GetParameters() =>
			GetParameters(LeftSpecification).Union(GetParameters(RightSpecification));

		private static IEnumerable<DslQueryParameter> GetParameters(IFilterSpecification specification) =>
			specification == null
				? Enumerable.Empty<DslQueryParameter>()
				: specification.GetParameters();
	}
}