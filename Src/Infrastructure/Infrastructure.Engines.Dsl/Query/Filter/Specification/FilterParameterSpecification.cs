namespace Infrastructure.Engines.Dsl.Query.Filter.Specification
{
	using System.Collections.Generic;

	public sealed class FilterParameterSpecification : FilterConstantSpecification
	{
		public override IEnumerable<DslQueryParameter> GetParameters()
		{
			yield return new DslQueryParameter(Value);
		}
	}
}