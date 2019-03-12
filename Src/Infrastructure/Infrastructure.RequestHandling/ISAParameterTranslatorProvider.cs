namespace Infrastructure.RequestHandling
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	/// <summary>
	///     Provides methods to get parameter translator.
	/// </summary>
	// ReSharper disable once InconsistentNaming
	internal interface ISAParameterTranslatorProvider
	{
		/// <summary>
		///     Gets the parameter translator by the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>
		///     The parameter translator
		/// </returns>
		[CanBeNull]
		ISAParameterTranslator Get([NotNull] string key, [NotNull] Dictionary<string, string> parameters);
	}
}