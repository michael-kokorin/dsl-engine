namespace Common.Licencing
{
	using System;

	using JetBrains.Annotations;

	using Common.Logging;
	using Common.Settings;

	[UsedImplicitly]
	internal sealed class ConfigInstallationLicenceIdProvider : IInstallationLicenceIdProvider
	{
		private readonly IConfigManager _configManager;

		private readonly ILog _log;

		public ConfigInstallationLicenceIdProvider(
			[NotNull] IConfigManager configManager,
			[NotNull] ILog log)
		{
			if (configManager == null)
				throw new ArgumentNullException(nameof(configManager));
			if (log == null) throw new ArgumentNullException(nameof(log));

			_configManager = configManager;
			_log = log;
		}

		public string GetInstallationLicenceId()
		{
			var currentLicenceId = _configManager.Get(Properties.Settings.Default.SystemLicenceId);

			_log.Trace($"Installation licence Id resolved. Licence Id='{currentLicenceId}'");

			return currentLicenceId;
		}
	}
}