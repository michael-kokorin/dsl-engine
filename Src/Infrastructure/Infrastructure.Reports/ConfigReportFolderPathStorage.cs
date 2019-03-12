namespace Infrastructure.Reports
{
	using System;
	using System.IO;

	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class ConfigReportFolderPathStorage : IReportFolderPathStorage
	{
		private const string ReportDefaultFolderName = "Reports";

		private readonly IConfigurationProvider _configurationProvider;

		public ConfigReportFolderPathStorage([NotNull] IConfigurationProvider configurationProvider)
		{
			if (configurationProvider == null) throw new ArgumentNullException(nameof(configurationProvider));

			_configurationProvider = configurationProvider;
		}

		public string GetReportFolderPath()
		{
			var reportRootDir = _configurationProvider.GetValue(ConfigurationKeys.AppSettings.ReportLogDir);

			if (!string.IsNullOrWhiteSpace(reportRootDir)) return reportRootDir;

			var tempDiePath = _configurationProvider.GetValue(ConfigurationKeys.AppSettings.TempDirPath)
			                  ?? Environment.CurrentDirectory;

			reportRootDir = Path.Combine(tempDiePath, ReportDefaultFolderName);

			_configurationProvider.SetValue(ConfigurationKeys.AppSettings.ReportLogDir, reportRootDir);

			return reportRootDir;
		}
	}
}