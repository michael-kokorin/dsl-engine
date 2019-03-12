namespace Infrastructure.Plugins.Tests
{
	using System.Collections.Generic;
	using System.Linq;

	using Moq;

	using NUnit.Framework;

	using Common.Enums;
	using Common.Logging;
	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;
	using Repository.Context;
	using Repository.Repositories;

	using PluginSettingDefinition = Infrastructure.Plugins.Contracts.PluginSettingDefinition;

	[TestFixture]
	public sealed class PluginSettingProviderTests
	{
		[SetUp]
		public void SetUp()
		{
			_log = new Mock<ILog>();
			_pluginRepository = new Mock<IPluginRepository>();
			_settingGroupRepository = new Mock<ISettingGroupRepository>();
			_settingRepository = new Mock<ISettingRepository>();
			_settingValuesRepository = new Mock<ISettingValuesRepository>();

			_target = new PluginSettingProvider(
				_log.Object,
				_pluginRepository.Object, _settingGroupRepository.Object, _settingRepository.Object, _settingValuesRepository.Object);

			_testPlugin = new Mock<ICorePlugin>();
		}

		private IPluginSettingProvider _target;

		private Mock<ILog> _log;

		private Mock<IPluginRepository> _pluginRepository;

		private Mock<ISettingGroupRepository> _settingGroupRepository;

		private Mock<ISettingValuesRepository> _settingValuesRepository;

		private Mock<ISettingRepository> _settingRepository;

		private Mock<ICorePlugin> _testPlugin;

		[Test]
		public void ShouldInsertNewPluginSetting()
		{
			_testPlugin.Setup(_ => _.GetSettings())
						.Returns(
							new PluginSettingGroupDefinition
							{
								Code = "1111",
								DisplayName = "2222",
								SettingDefinitions = new List<Common.Contracts.PluginSettingDefinition>
								{
									new Common.Contracts.PluginSettingDefinition
									{
										SettingOwner = SettingOwner.Project,
										DisplayName = "3333",
										Code = "4444",
										DefaultValue = "5555",
										IsAuthentication = false,
										SettingType = SettingType.Text
									}
								}
							});

			_pluginRepository
				.Setup(_ => _.GetByType(
					_testPlugin.Object.GetType().FullName,
					_testPlugin.Object.GetType().Assembly.GetName().Name,
					PluginType.IssueTracker))
				.Returns(new Plugins
				{
					Id = 23
				});

			_target.Initialize(_testPlugin.Object, PluginType.IssueTracker);

			_pluginRepository.Verify(_ => _.GetByType(
				_testPlugin.Object.GetType().FullName,
				_testPlugin.Object.GetType().Assembly.GetName().Name,
				PluginType.IssueTracker),
				Times.Once);

			_settingGroupRepository.Verify(_ => _.Insert(It.IsAny<SettingGroups>()), Times.Once);
			_settingRepository.Verify(_ => _.Insert(It.IsAny<Settings>()), Times.Once);
		}

		[Test]
		public void ShouldNotInsertExistsPluginSetting()
		{
			const string settingKey = "set_key";

			_testPlugin.Setup(_ => _.GetSettings())
				.Returns(new PluginSettingGroupDefinition
				{
					Code = "1111",
					DisplayName = "2222",
					SettingDefinitions = new List<Common.Contracts.PluginSettingDefinition>
								{
									new Common.Contracts.PluginSettingDefinition
									{
										SettingOwner = SettingOwner.Project,
										DisplayName = "3333",
										Code = settingKey,
										DefaultValue = "5555",
										IsAuthentication = false,
										SettingType = SettingType.Text
									}
								}
				});

			const int pluginId = 2134;

			_pluginRepository
				.Setup(_ => _.GetByType(
					_testPlugin.Object.GetType().FullName,
					_testPlugin.Object.GetType().Assembly.GetName().Name,
					PluginType.IssueTracker))
				.Returns(new Plugins
				{
					Id = pluginId
				});

			_settingRepository
				.Setup(_ => _.Get(pluginId, (int)SettingOwner.Project, settingKey))
				.Returns(new Settings
				{
					SettingOwner = (int)SettingOwner.Project,
					Code = settingKey,
					OwnerPluginId = pluginId
				});

			_target.Initialize(_testPlugin.Object, PluginType.IssueTracker);

			_settingRepository.Verify(_ => _.Insert(It.IsAny<Settings>()), Times.Never);
		}
	}
}