namespace Infrastructure.Engines.Dsl
{
	/// <summary>
	///     Represents policy rule expression.
	/// </summary>
	public sealed class PolicyRuleExpr
	{
		/// <summary>
		///     Gets or sets the data.
		/// </summary>
		/// <value>
		///     The data.
		/// </value>
		public DataParameterExpr[] Data { get; internal set; }
	}
}