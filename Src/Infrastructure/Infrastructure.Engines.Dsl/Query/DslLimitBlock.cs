namespace Infrastructure.Engines.Dsl.Query
{
	public sealed class DslLimitBlock : IDslQueryBlock
	{
		public int? Skip { get; set; }

		public int? Take { get; set; }
	}
}