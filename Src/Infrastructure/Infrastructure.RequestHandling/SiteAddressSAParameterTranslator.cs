namespace Infrastructure.RequestHandling
{
	using System.Collections.Generic;

	// ReSharper disable once InconsistentNaming
	internal sealed class SiteAddressSAParameterTranslator: ISAParameterTranslator
	{
		/// <summary>
		///     Gets the key.
		/// </summary>
		/// <value>
		///     The key.
		/// </value>
		public string Key => "site-address";

		/// <summary>
		///     Initializes this instance with the specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		public void Initialize(Dictionary<string, string> parameters)
		{
		}

		/// <summary>
		///     Translates the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The translated value.</returns>
		public string Translate(string value) => $"--host \"{value}\"";
	}
}