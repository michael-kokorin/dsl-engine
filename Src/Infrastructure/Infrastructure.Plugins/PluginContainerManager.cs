using Common.Container;
using Common.Extensions;
using Common.Logging;

namespace Infrastructure.Plugins
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;
	using Infrastructure.Plugins.Extensions;

	[UsedImplicitly]
	internal sealed class PluginContainerManager: IPluginContainerManager
	{
		private static readonly IDictionary<string, IUnityContainer> Containers
			= new Dictionary<string, IUnityContainer>();

		private readonly ILog _log;

		public PluginContainerManager([NotNull] ILog log)
		{
			if(log == null)
			{
				throw new ArgumentNullException(nameof(log));
			}

			_log = log;
		}

		public void Register(IPlugin plugin, string pluginTypeName)
		{
			if(plugin == null)
			{
				throw new ArgumentNullException(nameof(plugin));
			}

			if(string.IsNullOrEmpty(pluginTypeName))
			{
				throw new ArgumentNullException(nameof(pluginTypeName));
			}

			if(Containers.ContainsKey(pluginTypeName))
			{
				_log.Warning(Resources.Resources.PluginContainerManger_Register_PluginAlreadyRegistered.FormatWith(pluginTypeName));

				return;
			}

			var container = new UnityContainer();

			container.RegisterCommonPlugin(plugin, pluginTypeName, ReuseScope.PerResolve);

			if(plugin is ICorePlugin)
			{
				container.RegisterPlugin(plugin, pluginTypeName, ReuseScope.PerResolve);
			}

			Containers.Add(pluginTypeName, container);

			_log.Debug(Resources.Resources.PluginContainerManger_Register_PluginRegistered.FormatWith(pluginTypeName));
		}

		public ICorePlugin Resolve(string pluginTypeName)
		{
			if(string.IsNullOrEmpty(pluginTypeName))
			{
				throw new ArgumentNullException(nameof(pluginTypeName));
			}

			if(!Containers.ContainsKey(pluginTypeName))
			{
				throw new ArgumentException(nameof(pluginTypeName));
			}

			var pluginContainer = Containers[pluginTypeName];

			var plugin = pluginContainer.Resolve<ICorePlugin>(pluginTypeName);

			_log.Debug(Resources.Resources.PluginContainerManger_Resolve_PluginResolved.FormatWith(pluginTypeName));

			return plugin;
		}
	}
}