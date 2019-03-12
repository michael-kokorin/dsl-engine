namespace Modules.SA
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Microsoft.Practices.ObjectBuilder2;

	using Infrastructure.Plugins.Common;
	using Infrastructure.Plugins.Agent.Client.Contracts;

	/// <summary>
	///     Provides methods to manage plugins for scan agent.
	/// </summary>
	/// <seealso cref="Modules.SA.IPluginProvider"/>
	public sealed class PluginProvider: IPluginProvider
	{
		private readonly IPluginLoader<IAgentClientPlugin> _loader;

		private readonly Dictionary<string, IAgentClientPlugin> _plugins = new Dictionary<string, IAgentClientPlugin>();

		public PluginProvider([NotNull] IPluginLoader<IAgentClientPlugin> loader)
		{
			if(loader == null)
			{
				throw new ArgumentNullException(nameof(loader));
			}

			_loader = loader;
		}

		/// <summary>
		///     Gets all available plugin keys.
		/// </summary>
		/// <returns>All available plugin keys.</returns>
		public IEnumerable<string> GetAllAvailablePluginKeys() => _plugins.Keys;

		/// <summary>
		///     Gets plugin instance by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Plugin instance.</returns>
		public IAgentClientPlugin GetByKey(string key) => _plugins.ContainsKey(key) ? _plugins[key] : null;

		/// <summary>
		///     Initializes this instance.
		/// </summary>
		public void Initialize()
		{
			_plugins.Clear();
			var items = _loader.Load();
			items.ForEach(
				plugin =>
				{
					plugin.Initialize();
					_plugins.Add(plugin.Key, plugin);
				});
		}
	}
}