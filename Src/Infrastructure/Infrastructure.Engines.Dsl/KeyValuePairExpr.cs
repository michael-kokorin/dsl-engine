namespace Infrastructure.Engines.Dsl
{
	/// <summary>
	///   Represents key-value pair expression.
	/// </summary>
	public sealed class KeyValuePairExpr
	{
		/// <summary>
		///   Initializes a new instance of the <see cref="KeyValuePairExpr"/> class.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public KeyValuePairExpr(string key, string value)
		{
			Key = key;
			Value = value;
		}

		/// <summary>
		///   Gets the key.
		/// </summary>
		/// <value>
		///   The key.
		/// </value>
		public string Key { get; private set; }

		/// <summary>
		///   Gets the value.
		/// </summary>
		/// <value>
		///   The value.
		/// </value>
		public string Value { get; private set; }
	}
}