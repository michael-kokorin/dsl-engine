namespace Infrastructure.RequestHandling
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	/// <summary>
	///     Provides methods to translate parameter for scan agent.
	/// </summary>
	// ReSharper disable once InconsistentNaming
	internal interface ISAParameterTranslator
	{
		/// <summary>
		///     Gets the key.
		/// </summary>
		/// <value>
		///     The key.
		/// </value>
		[NotNull]
		string Key { get; }

		/// <summary>
		///     Initializes this instance with the specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		void Initialize([NotNull] Dictionary<string, string> parameters);

		/// <summary>
		///     Translates the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The translated value.</returns>
		[CanBeNull]
		string Translate([CanBeNull] string value);
	}
}