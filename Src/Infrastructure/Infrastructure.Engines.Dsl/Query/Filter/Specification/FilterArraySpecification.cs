namespace Infrastructure.Engines.Dsl.Query.Filter.Specification
{
	using System.Collections.Generic;
	using System.Linq;

	public sealed class FilterArraySpecification : IFilterSpecification
	{
		public IEnumerable<IFilterSpecification> Specifications { get; set; }

		public IEnumerable<DslQueryParameter> GetParameters() =>
			Specifications.SelectMany(specification => specification.GetParameters());
	}
}