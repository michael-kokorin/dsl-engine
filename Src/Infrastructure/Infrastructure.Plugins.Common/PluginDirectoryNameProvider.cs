using Common.Properties;
using Common.Settings;

namespace Infrastructure.Plugins.Common
{
	using System;
	using System.IO;
	using System.Web;

	using JetBrains.Annotations;

	internal sealed class PluginDirectoryNameProvider: IPluginDirectoryNameProvider
	{
		private readonly IConfigManager _configManager;

		public PluginDirectoryNameProvider([NotNull] IConfigManager configManager)
		{
			if(configManager == null)
			{
				throw new ArgumentNullException(nameof(configManager));
			}

			_configManager = configManager;
		}

		public string GetDirectory(bool writeToConfig = false)
		{
			var folderName = _configManager.Get(Settings.Default.PluginsFolderSettingName);

			if(!string.IsNullOrEmpty(folderName))
			{
				return folderName;
			}

			var localPath = HttpContext.Current?.Server.MapPath("/Plugins");

			if(string.IsNullOrEmpty(localPath))
			{
				localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
			}

			if(writeToConfig)
			{
				_configManager.Set(Settings.Default.PluginsFolderSettingName, localPath);
			}

			return localPath;
		}
	}
}