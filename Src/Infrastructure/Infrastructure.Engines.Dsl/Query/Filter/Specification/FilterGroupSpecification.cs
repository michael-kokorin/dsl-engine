namespace Infrastructure.Engines.Dsl.Query.Filter.Specification
{
	using System.Collections.Generic;
	using System.Linq;

	public sealed class FilterGroupSpecification : IFilterSpecification
	{
		public IFilterSpecification Specification { get; set; }

		public IEnumerable<DslQueryParameter> GetParameters() =>
			Specification == null
				? Enumerable.Empty<DslQueryParameter>()
				: Specification.GetParameters();
	}
}