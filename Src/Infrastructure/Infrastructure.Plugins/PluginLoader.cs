namespace Infrastructure.Plugins
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition.Hosting;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Contracts;

	internal sealed class PluginLoader : IPluginLoader
	{
		private readonly IPluginDirectoriesProvider _pluginDirectoriesProvider;

		public PluginLoader([NotNull] IPluginDirectoriesProvider pluginDirectoriesProvider)
		{
			if (pluginDirectoriesProvider == null) throw new ArgumentNullException(nameof(pluginDirectoriesProvider));

			_pluginDirectoriesProvider = pluginDirectoriesProvider;
		}

		public IEnumerable<IPlugin> Load()
		{
			var plugins = new List<IPlugin>();

			var directories = _pluginDirectoriesProvider.GetPluginDirectories();

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var directory in directories)
			{
				var catalog = new DirectoryCatalog(directory);

				var container = new CompositionContainer(catalog);

				var directoryPlugins = container
					.GetExports<IPlugin>();

				plugins.AddRange(directoryPlugins.Select(_ => _.Value));
			}

			return plugins;
		}
	}
}