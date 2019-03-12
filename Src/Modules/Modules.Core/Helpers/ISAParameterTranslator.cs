namespace Modules.Core.Helpers
{
	/// <summary>
	///   Provides methods to translate parameter for scan agent.
	/// </summary>
	// ReSharper disable once InconsistentNaming
	internal interface ISAParameterTranslator
	{
		/// <summary>
		///   Gets the key.
		/// </summary>
		/// <value>
		///   The key.
		/// </value>
		string Key { get; }

		/// <summary>
		///   Translates the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The translated value.</returns>
		string Translate(string value);
	}
}