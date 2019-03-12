namespace Plugins.GitHub.Load.Tests
{
	using System;
	using System.IO;
	using System.Linq;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.Plugins;
	using Infrastructure.Plugins.Common;
	using Infrastructure.Plugins.Contracts;

	[TestFixture]
	[Ignore("Integration test")]
	public sealed class PluginActivatorTest
	{
		private Mock<IPluginDirectoryNameProvider> _pluginDirectoryNameProvider;

		private IPluginDirectoriesProvider _pluginDirectoriesProvider;

		private IPluginLoader<ICorePlugin> _pluginLoader;

		[SetUp]
		public void SetUp()
		{
			_pluginDirectoryNameProvider = new Mock<IPluginDirectoryNameProvider>();

			_pluginDirectoryNameProvider.Setup(_ => _.GetDirectory(false))
				.Returns(Path.Combine(Environment.CurrentDirectory, "Plugins"));

			_pluginDirectoriesProvider = new PluginDirectoriesProvider(_pluginDirectoryNameProvider.Object);

			_pluginLoader = new PluginLoader<ICorePlugin>(_pluginDirectoriesProvider);
		}

		[Test]
		public void ShouldLoadItPlugin()
		{
			var result = _pluginLoader.Load().ToArray();

			result.Should().NotBeNullOrEmpty();
		}
	}
}