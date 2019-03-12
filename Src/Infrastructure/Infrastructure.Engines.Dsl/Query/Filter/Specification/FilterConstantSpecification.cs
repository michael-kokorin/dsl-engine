namespace Infrastructure.Engines.Dsl.Query.Filter.Specification
{
	using System.Collections.Generic;
	using System.Linq;

	public class FilterConstantSpecification : IFilterSpecification
	{
		public string Value { get; set; }

		public virtual IEnumerable<DslQueryParameter> GetParameters() => Enumerable.Empty<DslQueryParameter>();
	}
}