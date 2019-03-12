namespace Infrastructure.Engines.Dsl.Query.Filter
{
	using Infrastructure.Engines.Dsl.Query.Filter.Specification;

	/// <summary>
	///     Represents where expression.
	/// </summary>
	public sealed class DslFilterBlock : IDslQueryBlock
	{
		/// <summary>
		///     Gets or sets the condition.
		/// </summary>
		/// <value>
		///     The condition.
		/// </value>
		public IFilterSpecification Specification { get; set; }
	}
}