namespace Infrastructure.Engines.Dsl.Query
{
	using System.Collections.Generic;

	/// <summary>
	///     Represents select expression.
	/// </summary>
	public sealed class DslFormatBlock : IDslQueryBlock
	{
		/// <summary>
		///     Gets or sets the selects.
		/// </summary>
		/// <value>
		///     The selects.
		/// </value>
		public IEnumerable<DslFormatItem> Selects { get; set; }
	}
}