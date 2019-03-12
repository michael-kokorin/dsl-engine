namespace Infrastructure.Engines.Dsl.Query.Filter
{
	using System.ComponentModel;

	public enum FilterCondition
	{
		[Description(@"[&][&]")] And,

		[Description(@"[|][|]")] Or
	}
}