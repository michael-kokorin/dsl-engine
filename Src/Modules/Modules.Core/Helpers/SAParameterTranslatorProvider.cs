namespace Modules.Core.Helpers
{
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.Practices.Unity;

	using Common.Extensions;

	// ReSharper disable once InconsistentNaming
	internal sealed class SAParameterTranslatorProvider : ISAParameterTranslatorProvider
	{
		private readonly Dictionary<string, ISAParameterTranslator> _translators;

		public SAParameterTranslatorProvider(IUnityContainer container)
		{
			_translators = container.ResolveAll<ISAParameterTranslator>().ToDictionary(_ => _.Key, _ => _);
		}

		/// <summary>
		///   Gets the parameter translator by the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The parameter translator</returns>
		public ISAParameterTranslator Get(string key) => _translators.Get(key);
	}
}