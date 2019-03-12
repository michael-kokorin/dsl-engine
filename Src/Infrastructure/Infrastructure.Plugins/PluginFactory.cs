using Common.Extensions;
using Common.Logging;

namespace Infrastructure.Plugins
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Contracts;

	internal sealed class PluginFactory: IPluginFactory
	{
		private readonly ILog _log;

		private readonly IPluginActivator _pluginActivator;

		private readonly IPluginSettingProvider _pluginSettingProvider;

		public PluginFactory(
			[NotNull] ILog log,
			[NotNull] IPluginActivator pluginActivator,
			[NotNull] IPluginSettingProvider pluginSettingProvider)
		{
			if(log == null)
			{
				throw new ArgumentNullException(nameof(log));
			}
			if(pluginActivator == null)
			{
				throw new ArgumentNullException(nameof(pluginActivator));
			}
			if(pluginSettingProvider == null)
			{
				throw new ArgumentNullException(nameof(pluginSettingProvider));
			}

			_log = log;
			_pluginActivator = pluginActivator;
			_pluginSettingProvider = pluginSettingProvider;
		}

		public ICorePlugin Prepare(long pluginId, long projectId, long? userId = null)
		{
			var plugin = _pluginActivator.Activate(pluginId);

			var settings = _pluginSettingProvider.GetSettingsForPlugin(pluginId, projectId, userId);

			if((settings == null) ||
				(settings.Count == 0))
			{
				return plugin;
			}

			plugin.LoadSettingValues(settings);

			_log.Trace(
				Resources.Resources.PluginFactory_Prepare_PluginPrepared.FormatWith(
							pluginId,
							projectId,
							userId));

			return plugin;
		}
	}
}