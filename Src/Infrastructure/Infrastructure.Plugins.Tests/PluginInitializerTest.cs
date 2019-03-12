namespace Infrastructure.Plugins.Tests
{
    using Moq;

    using NUnit.Framework;

    using Common.Logging;
    using Infrastructure.Plugins.Common;
    using Infrastructure.Plugins.Contracts;

    [TestFixture]
    public sealed class PluginInitializerTest
    {
        private IPluginInitializer _target;

        private Mock<IPluginLoader<ICorePlugin>> _pluginLoader;

        private Mock<IPluginProvider> _pluginProvider;

        [SetUp]
        public void SetUp()
        {
            var log = Mock.Of<ILog>();

            _pluginLoader = new Mock<IPluginLoader<ICorePlugin>>();
            _pluginProvider = new Mock<IPluginProvider>();

            _target = new PluginInitializer(
                log,
                _pluginLoader.Object,
                _pluginProvider.Object);
        }

        [Test]
        public void ShouldInitializePlugins()
        {
            var plugin = Mock.Of<ICorePlugin>();

            _pluginLoader.Setup(_ => _.Load()).Returns(new[] {plugin});

            _target.InitializePlugins();

            _pluginProvider.Verify(_ => _.Initialize(plugin), Times.Once);
        }
    }
}