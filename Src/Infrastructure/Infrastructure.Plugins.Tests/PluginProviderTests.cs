namespace Infrastructure.Plugins.Tests
{
	using System;

	using Moq;

	using NUnit.Framework;

	using Common.Enums;
	using Common.Licencing;
	using Common.Logging;
	using Infrastructure.Plugins.Contracts;
	using Repository.Context;
	using Repository.Repositories;

	[TestFixture]
	public sealed class PluginProviderTests
	{
		private IPluginProvider _target;

		private Mock<ILicenceProvider> _licenceProvider;

		private Mock<ILog> _log;

		private Mock<IPluginContainerManager> _pluginContainerProvider;

		private Mock<IPluginRepository> _pluginRepository;

		private Mock<IPluginSettingProvider> _pluginSettingProvider;

		private Mock<IVersionControlPlugin> _testPlugin;

		[SetUp]
		public void SetUp()
		{
			_testPlugin = new Mock<IVersionControlPlugin>();

			_licenceProvider = new Mock<ILicenceProvider>();

			var licence = new Mock<ILicence>();
			licence.Setup(_ => _.Get<PluginLicenceComponent>()).Returns(new PluginLicenceComponent());

			_licenceProvider.Setup(_ => _.GetCurrent()).Returns(licence.Object);

			_log = new Mock<ILog>();
			_pluginContainerProvider = new Mock<IPluginContainerManager>();
			_pluginRepository = new Mock<IPluginRepository>();
			_pluginSettingProvider = new Mock<IPluginSettingProvider>();

			_target = new PluginProvider(
				_licenceProvider.Object,
				_log.Object,
				_pluginContainerProvider.Object,
				_pluginRepository.Object,
				_pluginSettingProvider.Object);
		}

		[Test]
		public void ShouldNotSaveExistsPlugin()
		{
			_pluginRepository
				.Setup(_ => _.GetByType(
					_testPlugin.Object.GetType().FullName,
					_testPlugin.Object.GetType().Assembly.GetName().Name,
					PluginType.VersionControl))
				.Returns(new Plugins());

			_target.Initialize(_testPlugin.Object);

			_pluginRepository.Verify(_ => _.Insert(It.IsAny<Plugins>()), Times.Never);

			_pluginSettingProvider.Verify(_ => _.Initialize(_testPlugin.Object, PluginType.IssueTracker), Times.Never);

			_pluginContainerProvider.Verify(_ => _.Register(_testPlugin.Object, _testPlugin.Object.GetType().FullName),
				Times.Once);
		}

		[Test]
		public void ShouldSaveNewPlugin()
		{
			_target.Initialize(_testPlugin.Object);

			_pluginRepository.Verify(_ => _.Insert(It.IsAny<Plugins>()), Times.Once);

			_pluginSettingProvider.Verify(_ => _.Initialize(_testPlugin.Object, PluginType.VersionControl), Times.Once);

			_pluginContainerProvider.Verify(_ => _.Register(_testPlugin.Object, _testPlugin.Object.GetType().FullName),
				Times.Once);
		}

		[Test]
		public void ShouldThrowOnNullPlugin() => Assert.Throws<ArgumentNullException>(() => _target.Initialize(null));
	}
}