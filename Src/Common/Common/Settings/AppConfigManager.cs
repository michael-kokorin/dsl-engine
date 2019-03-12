namespace Common.Settings
{
	using System;
	using System.Configuration;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Logging;
	using Common.Properties;

	internal sealed class AppConfigManager: IConfigManager
	{
		private readonly ILog _log;

		public AppConfigManager([NotNull] ILog log)
		{
			if(log == null) throw new ArgumentNullException(nameof(log));

			_log = log;
		}

		/// <summary>
		///   Gets value of the specified setting.
		/// </summary>
		/// <param name="settingName">Name of the setting.</param>
		/// <returns>
		///   The value of the specified setting.
		/// </returns>
		public string Get(string settingName) => ConfigurationManager.AppSettings[settingName];

		/// <summary>
		///   Gets the connection string.
		/// </summary>
		/// <returns>
		///   The connection string.
		/// </returns>
		public string GetConnectionString()
			=> ConfigurationManager.ConnectionStrings[Settings.Default.ContextName]?.ConnectionString;

		/// <summary>
		///   Sets the specified setting.
		/// </summary>
		/// <param name="settingName">Name of the setting.</param>
		/// <param name="value">The value.</param>
		public void Set(string settingName, string value)
		{
			var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

			if (configManager.AppSettings.Settings[settingName] != null)
				configManager.AppSettings.Settings.Remove(settingName);

			configManager.AppSettings.Settings.Add(settingName, value);

			configManager.Save();

			ConfigurationManager.RefreshSection("appSettings");

			_log.Info(
				Resources.ApplicationConfigurationProvider_SetConfig_ConfigurationUpdated.FormatWith(
					settingName,
					value));
		}
	}
}