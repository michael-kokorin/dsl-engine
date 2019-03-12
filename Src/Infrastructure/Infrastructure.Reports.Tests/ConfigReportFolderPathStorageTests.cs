namespace Infrastructure.Reports.Tests
{
	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	[TestFixture]
	public sealed class ConfigReportFolderPathStorageTests
	{
		private IReportFolderPathStorage _target;

		private Mock<IConfigurationProvider> _configurationProvider;

		[SetUp]
		public void SetUp()
		{
			_configurationProvider = new Mock<IConfigurationProvider>();

			_target = new ConfigReportFolderPathStorage(_configurationProvider.Object);
		}

		[Test]
		public void ShouldGetReportLogDir()
		{
			const string reportLogDir = "report log dir";

			_configurationProvider
				.Setup(_ => _.GetValue(ConfigurationKeys.AppSettings.ReportLogDir))
				.Returns(reportLogDir);

			var result = _target.GetReportFolderPath();

			result.ShouldBeEquivalentTo(reportLogDir);
		}

		[Test]
		public void ShouldGetReportLogDirFromTempPath()
		{
			const string tempDirPath = "tempDirPath";

			_configurationProvider
				.Setup(_ => _.GetValue(ConfigurationKeys.AppSettings.ReportLogDir))
				.Returns((string) null);

			_configurationProvider
				.Setup(_ => _.GetValue(ConfigurationKeys.AppSettings.TempDirPath))
				.Returns(tempDirPath);

			var reportDirPath = $"{tempDirPath}\\Reports";

			var result = _target.GetReportFolderPath();

			result.ShouldBeEquivalentTo(reportDirPath);

			_configurationProvider.Verify(_ => _.SetValue(ConfigurationKeys.AppSettings.ReportLogDir, reportDirPath), Times.Once);
		}
	}
}