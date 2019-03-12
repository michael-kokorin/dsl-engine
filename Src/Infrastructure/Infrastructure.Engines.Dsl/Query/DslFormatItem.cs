namespace Infrastructure.Engines.Dsl.Query
{
	/// <summary>
	///     Represents item in select expression.
	/// </summary>
	public sealed class DslFormatItem
	{
		/// <summary>
		///     Gets or sets the name.
		/// </summary>
		/// <value>
		///     The name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		///     Gets or sets the value.
		/// </summary>
		/// <value>
		///     The value.
		/// </value>
		public string Value { get; set; }


		/// <summary>
		/// Gets or sets the display name.
		/// </summary>
		/// <value>
		/// The display name.
		/// </value>
		public string DisplayName { get; set; }


		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		public string Description { get; set; }
	}
}