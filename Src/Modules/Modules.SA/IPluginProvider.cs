namespace Modules.SA
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Agent.Client.Contracts;

	/// <summary>
	///     Provides methods to get plugins.
	/// </summary>
	internal interface IPluginProvider
	{
		/// <summary>
		///     Gets all available plugin keys.
		/// </summary>
		/// <returns>All available plugin keys.</returns>
		[NotNull]
		[ItemNotNull]
		IEnumerable<string> GetAllAvailablePluginKeys();

		/// <summary>
		///     Gets plugin instance by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Plugin instance.</returns>
		[CanBeNull]
		IAgentClientPlugin GetByKey([NotNull] string key);

		/// <summary>
		///     Initializes this instance.
		/// </summary>
		void Initialize();
	}
}