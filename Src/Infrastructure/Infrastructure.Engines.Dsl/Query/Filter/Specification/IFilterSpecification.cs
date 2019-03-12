namespace Infrastructure.Engines.Dsl.Query.Filter.Specification
{
	using System.Collections.Generic;

	public interface IFilterSpecification
	{
		IEnumerable<DslQueryParameter> GetParameters();
	}
}