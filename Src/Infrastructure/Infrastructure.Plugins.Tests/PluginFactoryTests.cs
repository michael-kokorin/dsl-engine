namespace Infrastructure.Plugins.Tests
{
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Common;

    using Moq;

    using NUnit.Framework;

    using Common.Logging;
    using Infrastructure.Plugins.Contracts;

    [TestFixture]
    public sealed class PluginFactoryTests
    {
        [SetUp]
        public void SetUp()
        {
            _log = new Mock<ILog>();
            _pluginActivator = new Mock<IPluginActivator>();
            _pluginSettingProvider = new Mock<IPluginSettingProvider>();

            _target = new PluginFactory(_log.Object, _pluginActivator.Object, _pluginSettingProvider.Object);

            _plugin = new Mock<ICorePlugin>();
        }

        private IPluginFactory _target;

        private Mock<ILog> _log;

        private Mock<IPluginActivator> _pluginActivator;

        private Mock<IPluginSettingProvider> _pluginSettingProvider;

        private Mock<ICorePlugin> _plugin;

        [Test]
        public void ShouldPreparePlugin()
        {
            const long pluginId = 123;
            const long projectId = 432;
            const long userId = 523;

            var pluginObject = _plugin.Object;

            var settingsDictionary = new Dictionary<string, string>
            {
                {"param1", "value1"}
            };

            _pluginActivator
                .Setup(_ => _.Activate(pluginId))
                .Returns(pluginObject);

            _pluginSettingProvider
                .Setup(_ => _.GetSettingsForPlugin(pluginId, projectId, userId))
                .Returns(settingsDictionary);

            var result = _target.Prepare(pluginId, projectId, userId);

            result.Should().NotBeNull();
            result.IsSameOrEqualTo(pluginObject);

            _plugin.Verify(_ => _.LoadSettingValues(settingsDictionary), Times.Once);
        }
    }
}