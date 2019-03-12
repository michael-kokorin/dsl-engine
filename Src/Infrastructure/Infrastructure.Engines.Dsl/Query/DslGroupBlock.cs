namespace Infrastructure.Engines.Dsl.Query
{
	using System.Collections.Generic;

	public sealed class DslGroupBlock : IDslQueryBlock
	{
		public IEnumerable<DslGroupItem> Items { get; set; }
	}
}