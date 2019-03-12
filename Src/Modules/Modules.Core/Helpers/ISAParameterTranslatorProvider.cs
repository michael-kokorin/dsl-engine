namespace Modules.Core.Helpers
{
	/// <summary>
	///   Provides methods to get parameter translator.
	/// </summary>
	// ReSharper disable once InconsistentNaming
	internal interface ISAParameterTranslatorProvider
	{
		/// <summary>
		///   Gets the parameter translator by the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The parameter translator</returns>
		ISAParameterTranslator Get(string key);
	}
}