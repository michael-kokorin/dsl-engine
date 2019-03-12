namespace Infrastructure.Plugins.Tests
{
	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Licencing;
	using Common.Logging;
	using Infrastructure.Plugins.Contracts;
	using Repository.Context;
	using Repository.Repositories;

	[TestFixture]
	public sealed class PluginActivatorTest
	{
		private IPluginActivator _target;

		private Mock<ILicenceProvider> _licenceProvider;

		private Mock<ILog> _log;

		private Mock<IPluginContainerManager> _pluginContainerProvider;

		private Mock<IPluginRepository> _pluginRepository;

		[SetUp]
		public void SetUp()
		{
			var licence = new Mock<ILicence>();
			licence.Setup(_ => _.Get<PluginLicenceComponent>()).Returns(new PluginLicenceComponent());

			_licenceProvider = new Mock<ILicenceProvider>();
			_licenceProvider.Setup(_ => _.GetCurrent()).Returns(licence.Object);

			_log = new Mock<ILog>();
			_pluginRepository = new Mock<IPluginRepository>();
			_pluginContainerProvider = new Mock<IPluginContainerManager>();

			_target = new PluginActivator(
				_licenceProvider.Object,
				_log.Object,
				_pluginContainerProvider.Object,
				_pluginRepository.Object);
		}

		[Test]
		public void ShouldActivatePlugin()
		{
			const long pluginId = 234;
			const string pluginAssemblyName = "plugin_accembly_name";
			const string pluginTypeFullName = "plugin_type_full_name";

			_pluginRepository
				.Setup(_ => _.GetById(pluginId))
				.Returns(new Plugins
				{
					AssemblyName = pluginAssemblyName,
					Id = pluginId,
					TypeFullName = pluginTypeFullName
				});

			var plugin = new Mock<ICorePlugin>();

			_pluginContainerProvider
				.Setup(_ => _.Resolve(pluginTypeFullName))
				.Returns(plugin.Object);

			var result = _target.Activate(pluginId);

			result.Should().NotBeNull();
			result.ShouldBeEquivalentTo(plugin.Object);
		}
	}
}