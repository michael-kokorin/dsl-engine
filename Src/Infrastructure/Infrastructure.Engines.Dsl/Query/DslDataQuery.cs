namespace Infrastructure.Engines.Dsl.Query
{
	/// <summary>
	///     Represents data query expression.
	/// </summary>
	public sealed class DslDataQuery
	{
		/// <summary>
		///     Gets the name of the query entity.
		/// </summary>
		/// <value>
		///     The name of the query entity.
		/// </value>
		public string QueryEntityName { get; set; }

		/// <summary>
		/// Gets or sets the query blocks.
		/// </summary>
		/// <value>
		/// The query blocks.
		/// </value>
		public IDslQueryBlock[] Blocks { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether is table render required.
		/// </summary>
		/// <value>
		/// <c>true</c> if table render is required; otherwise, <c>false</c>.
		/// </value>
		public bool IsTableRenderRequired { get; set; }

		/// <summary>
		/// Gets or sets the parameters.
		/// </summary>
		/// <value>
		/// The parameters.
		/// </value>
		public DslQueryParameter[] Parameters { get; set; }

		/// <summary>
		///     Gets the table key.
		/// </summary>
		/// <value>
		///     The table key.
		/// </value>
		public string TableKey { get; set; }

		/// <summary>
		///     Gets a value indicating whether take first option is specified.
		/// </summary>
		/// <value>
		///     <see langword="true" /> if take first option is specified; otherwise, <see langword="false" />.
		/// </value>
		public bool TakeFirst { get; set; }

		/// <summary>
		///     Gets a value indicating whether take first or default option is specified.
		/// </summary>
		/// <value>
		///     <see langword="true" /> if take first or default option is specified; otherwise, <see langword="false" />.
		/// </value>
		public bool TakeFirstOrDefault { get; set; }
	}
}