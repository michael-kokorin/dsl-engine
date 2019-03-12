namespace Modules.Core.Helpers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class SiteAddressSAParameterTranslator : ISAParameterTranslator
	{
		/// <summary>
		///   Gets the key.
		/// </summary>
		/// <value>
		///   The key.
		/// </value>
		public string Key => "site-address";

		/// <summary>
		///   Translates the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The translated value.</returns>
		public string Translate(string value) => $"--host \"{value}\"";
	}
}