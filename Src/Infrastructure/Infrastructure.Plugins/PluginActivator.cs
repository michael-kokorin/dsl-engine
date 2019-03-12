using Common.Extensions;
using Common.Licencing;
using Common.Logging;

namespace Infrastructure.Plugins
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Common;
	using Infrastructure.Plugins.Contracts;
	using Repository.Repositories;

	internal sealed class PluginActivator: IPluginActivator
	{
		private readonly ILicenceProvider _licenseProvider;

		private readonly ILog _log;

		private readonly IPluginContainerManager _pluginContainerManager;

		private readonly IPluginRepository _pluginRepository;

		public PluginActivator(
			[NotNull] ILicenceProvider licenseProvider,
			[NotNull] ILog log,
			[NotNull] IPluginContainerManager pluginContainerManager,
			[NotNull] IPluginRepository pluginRepository)
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

			_licenseProvider = licenseProvider;
			_log = log;
			_pluginContainerManager = pluginContainerManager;
			_pluginRepository = pluginRepository;
		}

		public ICorePlugin Activate(long pluginId)
		{
			var plugin = _pluginRepository.GetById(pluginId);

			if(plugin == null)
			{
				throw new ArgumentException(nameof(pluginId));
			}

			var currentLicense = _licenseProvider.GetCurrent();

			var pluginLicenseComponent = currentLicense.Get<PluginLicenceComponent>();

			var pluginTypeName = plugin.TypeFullName;

			if(!pluginLicenseComponent.CanUse(pluginTypeName))
			{
				throw new PluginActivationDeniedByLicenseException(currentLicense.Id, pluginTypeName);
			}

			var pluginInstance = _pluginContainerManager.Resolve(pluginTypeName);

			_log.Debug(Resources.Resources.PluginActivator_Activate_PluginActivated.FormatWith(pluginId, pluginTypeName));

			return pluginInstance;
		}
	}
}