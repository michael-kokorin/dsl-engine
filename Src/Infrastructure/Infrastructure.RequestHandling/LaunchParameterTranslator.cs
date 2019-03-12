namespace Infrastructure.RequestHandling
{
	using System.Collections.Generic;

	public sealed class LaunchParameterTranslator: ISAParameterTranslator
	{
		/// <summary>
		///     Gets the key.
		/// </summary>
		/// <value>
		///     The key.
		/// </value>
		public string Key => "launch-parameters";

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
		public string Translate(string value) => value;
	}
}