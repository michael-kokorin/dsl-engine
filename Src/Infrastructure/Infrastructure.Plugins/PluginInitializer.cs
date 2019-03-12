using Common.Extensions;
using Common.Logging;

namespace Infrastructure.Plugins
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Common;
	using Infrastructure.Plugins.Common.Contracts;

	internal sealed class PluginInitializer: IPluginInitializer
	{
		private static readonly object PluginInitializationLocker = new object();

		private readonly ILog _log;

		private readonly IPluginLoader<IPlugin> _pluginLoader;

		private readonly IPluginProvider _pluginProvider;

		public PluginInitializer(
			[NotNull] ILog log,
			[NotNull] IPluginLoader<IPlugin> pluginLoader,
			[NotNull] IPluginProvider pluginProvider)
		{
			if(log == null)
			{
				throw new ArgumentNullException(nameof(log));
			}

			if(pluginLoader == null)
			{
				throw new ArgumentNullException(nameof(pluginLoader));
			}

			if(pluginProvider == null)
			{
				throw new ArgumentNullException(nameof(pluginProvider));
			}

			_log = log;
			_pluginLoader = pluginLoader;
			_pluginProvider = pluginProvider;
		}

		public void InitializePlugins()
		{
			lock(PluginInitializationLocker)
			{
				_log.Trace(Resources.Resources.PluginInitializer_InitializePlugins_Plugins_initialization_started);

				var pluginInstances = _pluginLoader.Load();

				var initialized = 0;

				foreach(var plugin in pluginInstances)
				{
					_pluginProvider.Initialize(plugin);

					initialized++;
				}

				_log.Debug(
					Resources.Resources.PluginInitializer_InitializePlugins_Plugins_Initialization_finished.FormatWith(initialized));
			}
		}
	}
}