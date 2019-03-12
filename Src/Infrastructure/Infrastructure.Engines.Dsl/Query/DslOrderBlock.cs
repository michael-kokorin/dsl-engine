namespace Infrastructure.Engines.Dsl.Query
{
	/// <summary>
	///     Describes order expression.
	/// </summary>
	public sealed class DslOrderBlock : IDslQueryBlock
	{
		/// <summary>
		///     Gets or sets the items.
		/// </summary>
		/// <value>
		///     The items.
		/// </value>
		public OrderBlockItem[] Items { get; set; }
	}
}