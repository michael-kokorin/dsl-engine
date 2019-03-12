namespace Infrastructure.Plugins.Tests
{
	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Properties;
	using Common.Settings;
	using Infrastructure.Plugins.Common;

	[TestFixture]
	public sealed class PluginDirectoryNameProviderTest
	{
		[SetUp]
		public void SetUp()
		{
			_applicationConfigurationProvider = new Mock<IConfigManager>();

			_target = new PluginDirectoryNameProvider(_applicationConfigurationProvider.Object);
		}

		private Mock<IConfigManager> _applicationConfigurationProvider;

		private IPluginDirectoryNameProvider _target;

		[Test]
		public void ShouldReturnDirectoryNameFromApplicationConfig()
		{
			const string configPath = "config_path";

			_applicationConfigurationProvider
				.Setup(_ => _.Get(Settings.Default.PluginsFolderSettingName))
				.Returns(configPath);

			var result = _target.GetDirectory();

			result.Should().NotBeNullOrEmpty();
			result.ShouldAllBeEquivalentTo(configPath);
		}
	}
}