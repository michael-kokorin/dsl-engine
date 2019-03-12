namespace Infrastructure.AD
{
	using System;

	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class ActiveDirectoryPathProvider : IActiveDirectoryPathProvider
	{
		private readonly IConfigurationProvider _configurationProvider;

		public ActiveDirectoryPathProvider([NotNull] IConfigurationProvider configurationProvider)
		{
			_configurationProvider = configurationProvider;
		}

		public string GetPath()
		{
			var path = _configurationProvider.GetValue(ConfigurationKeys.AppSettings.ActiveDirectoryRootGroup);

			if (!string.IsNullOrEmpty(path)) return path;

			path = "WinNT://" + Environment.MachineName + ",computer";

			_configurationProvider.SetValue(ConfigurationKeys.AppSettings.ActiveDirectoryRootGroup, path);

			return path;
		}
	}
}