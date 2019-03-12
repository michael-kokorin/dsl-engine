namespace Infrastructure.Plugins.Common
{
	using System.Collections.Generic;
	using System.ComponentModel.Composition.Hosting;
	using System.Linq;

	using Infrastructure.Plugins.Common.Contracts;

	/// <summary>
	///     Provides methods to load plugins of specified type.
	/// </summary>
	/// <typeparam name="TPlugin">The type of the plugin.</typeparam>
	/// <seealso cref="Infrastructure.Plugins.Common.IPluginLoader{TPlugin}"/>
	public sealed class PluginLoader<TPlugin> : IPluginLoader<TPlugin> where TPlugin : IPlugin
	{
		private readonly IPluginDirectoriesProvider _pluginDirectoriesProvider;

		public PluginLoader(IPluginDirectoriesProvider pluginDirectoriesProvider)
		{
			_pluginDirectoriesProvider = pluginDirectoriesProvider;
		}

		/// <summary>
		///     Loads plugins from file share.
		/// </summary>
		/// <returns>Collection of plugins.</returns>
		public IEnumerable<TPlugin> Load()
		{
			var plugins = new List<TPlugin>();

			var directories = _pluginDirectoriesProvider.GetPluginDirectories();

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var directory in directories)
			{
				var catalog = new DirectoryCatalog(directory);

				var container = new CompositionContainer(catalog);

				var directoryPlugins = container
					.GetExports<TPlugin>();

				plugins.AddRange(directoryPlugins.Select(_ => _.Value));
			}

			return plugins;
		}
	}
}