namespace Infrastructure.Engines.Dsl
{
	/// <summary>
	///     Represents the data parameter expression.
	/// </summary>
	public sealed class DataParameterExpr
	{
		/// <summary>
		///     Gets or sets the name.
		/// </summary>
		/// <value>
		///     The name.
		/// </value>
		public string Name { get; internal set; }

		/// <summary>
		///     Gets or sets the query.
		/// </summary>
		/// <value>
		///     The query.
		/// </value>
		public string Value { get; internal set; }
	}
}