using Common.Enums;
using Common.Extensions;
using Common.Licencing;
using Common.Logging;

namespace Infrastructure.Plugins
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	using JetBrains.Annotations;


	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;
	using Repository.Context;
	using Repository.Repositories;

	internal sealed class PluginProvider: IPluginProvider
	{
		private readonly ILicenceProvider _licenseProvider;

		private readonly ILog _log;

		private readonly IPluginContainerManager _pluginContainerManager;

		private readonly IPluginRepository _pluginRepository;

		private readonly IPluginSettingProvider _pluginSettingProvider;

		public PluginProvider(
			[NotNull] ILicenceProvider licenseProvider,
			[NotNull] ILog log,
			[NotNull] IPluginContainerManager pluginContainerManager,
			[NotNull] IPluginRepository pluginRepository,
			[NotNull] IPluginSettingProvider pluginSettingProvider)
		{
			if(licenseProvider == null)
			{
				throw new ArgumentNullException(nameof(licenseProvider));
			}
			if(log == null)
			{
				throw new ArgumentNullException(nameof(log));
			}
			if(pluginContainerManager == null)
			{
				throw new ArgumentNullException(nameof(pluginContainerManager));
			}
			if(pluginRepository == null)
			{
				throw new ArgumentNullException(nameof(pluginRepository));
			}
			if(pluginSettingProvider == null)
			{
				throw new ArgumentNullException(nameof(pluginSettingProvider));
			}

			_licenseProvider = licenseProvider;
			_log = log;
			_pluginContainerManager = pluginContainerManager;
			_pluginRepository = pluginRepository;
			_pluginSettingProvider = pluginSettingProvider;
		}

		public void Initialize([NotNull] IPlugin plugin)
		{
			if(plugin == null)
			{
				throw new ArgumentNullException(nameof(plugin));
			}

			var assemblyName = plugin.GetType().Assembly.GetName().Name;
			var typeFullName = plugin.GetType().FullName;
			var pluginTypes = GetPluginTypes(plugin);

			var currentLicense = _licenseProvider.GetCurrent();

			var pluginLicenseComponent = currentLicense.Get<PluginLicenceComponent>();

			if(!pluginLicenseComponent.CanUse(typeFullName))
			{
				_log.Warning(
					$"Plugin type registration denied by licence. Licence Id='{currentLicense.Id}', Plugin type='{typeFullName}'");

				return;
			}

			if(pluginTypes.Length == 0)
			{
				_log.Warning(
					Resources.Resources.PluginProvider_Initialize_PluginNotImplementsInterfaces.FormatWith(
						typeFullName,
						assemblyName));

				return;
			}

			foreach(var pluginType in pluginTypes)
			{
				var pluginDb = _pluginRepository.GetByType(typeFullName, assemblyName, pluginType);

				if(pluginDb != null)
				{
					_log.Info(
						Resources.Resources.PluginProvider_Initialize_PluginAlreadyRegistered.FormatWith(
							typeFullName,
							assemblyName,
							pluginType));
				}
				else
				{
					pluginDb = new Plugins
								{
									AssemblyName = assemblyName,
									DisplayName = plugin.Title,
									TypeFullName = typeFullName,
									Type = (int)pluginType
								};

					_pluginRepository.Insert(pluginDb);

					_pluginRepository.Save();

					LogInitializedPlugin(pluginDb);
				}

				_pluginSettingProvider.Initialize(plugin, pluginType);

				_pluginContainerManager.Register(plugin, typeFullName);
			}
		}

		/// <summary>
		///     Gets plugins list.
		/// </summary>
		/// <returns>Plugins list.</returns>
		[NotNull]
		public IEnumerable<Plugins> Get([NotNull] Expression<Func<Plugins, bool>> specification)
		{
			var plugins = _pluginRepository.Query().Where(specification).ToArray();

			var currentLicense = _licenseProvider.GetCurrent();

			var pluginLicenseComponent = currentLicense.Get<PluginLicenceComponent>();

			return plugins.Where(_ => pluginLicenseComponent.CanUse(_.TypeFullName));
		}

		[NotNull]
		private static PluginType[] GetPluginTypes(IPlugin plugin)
		{
			var result = new List<PluginType>();

			if(plugin is IIssueTrackerPlugin)
			{
				result.Add(PluginType.IssueTracker);
			}

			if(plugin is IVersionControlPlugin)
			{
				result.Add(PluginType.VersionControl);
			}

			if(plugin is IAgentServerPlugin)
			{
				result.Add(PluginType.ServerAgent);
			}

			return result.ToArray();
		}

		private void LogInitializedPlugin([NotNull] Plugins plugin) => _log.Debug(
			Resources.Resources.PluginProvider_LogInitializedPlugin_PluginInitialized.FormatWith(
				plugin.AssemblyName,
				plugin.GetType().FullName,
				(PluginType)plugin.Type,
				plugin.Id));
	}
}