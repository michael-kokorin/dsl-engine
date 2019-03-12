using Common.Enums;
using Common.Extensions;
using Common.Logging;

namespace Infrastructure.Plugins
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Common.Contracts;
	using Repository.Context;
	using Repository.Repositories;

	internal sealed class PluginSettingProvider: IPluginSettingProvider
	{
		private readonly ILog _log;

		private readonly IPluginRepository _pluginRepository;

		private readonly ISettingGroupRepository _settingGroupRepository;

		private readonly ISettingRepository _settingRepository;

		private readonly ISettingValuesRepository _settingValuesRepository;

		public PluginSettingProvider(
			[NotNull] ILog log,
			[NotNull] IPluginRepository pluginRepository,
			[NotNull] ISettingGroupRepository settingGroupRepository,
			[NotNull] ISettingRepository settingRepository,
			[NotNull] ISettingValuesRepository settingValuesRepository)
		{
			if(log == null)
			{
				throw new ArgumentNullException(nameof(log));
			}

			if(pluginRepository == null)
			{
				throw new ArgumentNullException(nameof(pluginRepository));
			}

			if(settingGroupRepository == null)
			{
				throw new ArgumentNullException(nameof(settingGroupRepository));
			}
			if(settingRepository == null)
			{
				throw new ArgumentNullException(nameof(settingRepository));
			}
			if(settingValuesRepository == null)
			{
				throw new ArgumentNullException(nameof(settingValuesRepository));
			}

			_log = log;
			_pluginRepository = pluginRepository;
			_settingGroupRepository = settingGroupRepository;
			_settingRepository = settingRepository;
			_settingValuesRepository = settingValuesRepository;
		}

		public void Initialize([NotNull] IPlugin plugin, PluginType pluginType)
		{
			if(plugin == null)
			{
				throw new ArgumentNullException(nameof(plugin));
			}

			var fullTypeName = plugin.GetType().FullName;
			var assemblyName = plugin.GetType().Assembly.GetName().Name;

			var pluginDb = _pluginRepository.GetByType(fullTypeName, assemblyName, pluginType);

			if(pluginDb == null)
			{
				throw new Exception("Plugin is not found: {0} {1} {2}".FormatWith(fullTypeName, assemblyName, pluginType));
			}

			var pluginSettings = plugin.GetSettings();

			var settingKeys = new List<string>();
			HandleSettingGroup(pluginDb.Id, null, pluginSettings, settingKeys);

			var oldSettings = _settingRepository.Query()
												.Where(item => (item.OwnerPluginId == pluginDb.Id) && settingKeys.All(key => key != item.Code) && !item.IsArchived).ToList();
			foreach (var oldSetting in oldSettings)
			{
				oldSetting.IsArchived = true;
			}

			_settingRepository.Save();
		}

		public IDictionary<string, string> GetSettingsForPlugin(long pluginId, long projectId, long? userId = null)
		{
			var projectSettings =
				_settingValuesRepository.Query()
										.Where(
											item =>
												(item.Settings.OwnerPluginId == pluginId) && (item.Settings.SettingOwner == (int)SettingOwner.Project) &&
												(item.EntityId == projectId) && !item.Settings.IsArchived)
										.ToDictionary(item => item.Settings.Code, item => item.Value);

			if(!userId.HasValue)
			{
				return projectSettings;
			}

			var userSettings =
				_settingValuesRepository.Query()
										.Where(
											item =>
												(item.Settings.OwnerPluginId == pluginId) && (item.Settings.SettingOwner == (int)SettingOwner.User) &&
												(item.EntityId == userId.Value) && (item.ProjectId == projectId) && !item.Settings.IsArchived)
										.ToDictionary(item => item.Settings.Code, item => item.Value);

			return projectSettings.ConcatWith(userSettings);
		}

		private long HandleGroupItself(
			long pluginId,
			long? parentGroupId,
			[NotNull] PluginSettingGroupDefinition groupDefinition)
		{
			var groupDb = _settingGroupRepository.Get(pluginId, groupDefinition.Code);
			if(groupDb != null)
			{
				return groupDb.Id;
			}

			groupDb = new SettingGroups
			{
				Code = groupDefinition.Code,
				DisplayName = groupDefinition.DisplayName,
				OwnerPluginId = pluginId,
				ParentGroupId = parentGroupId
			};
			_settingGroupRepository.Insert(groupDb);
			_settingGroupRepository.Save();

			return groupDb.Id;
		}

		private void HandleSetting(long pluginId, long parentGroupId, [NotNull] PluginSettingDefinition settingDefinition)
		{
			var settingDb = _settingRepository.Get(pluginId, (int)settingDefinition.SettingOwner, settingDefinition.Code);
			if(settingDb != null)
			{
				return;
			}

			settingDb = new Settings
			{
				Code = settingDefinition.Code,
				DefaultValue = settingDefinition.DefaultValue,
				DisplayName = settingDefinition.DisplayName,
				IsAuth = settingDefinition.IsAuthentication,
				OwnerPluginId = pluginId,
				SettingGroupId = parentGroupId,
				SettingOwner = (int)settingDefinition.SettingOwner,
				SettingType = (int)settingDefinition.SettingType
			};

			_settingRepository.Insert(settingDb);

			if (settingDefinition.SettingOwner == SettingOwner.User)
			{
				settingDb = new Settings
				{
					Code = settingDefinition.Code,
					DefaultValue = settingDefinition.DefaultValue,
					DisplayName = settingDefinition.DisplayName,
					IsAuth = settingDefinition.IsAuthentication,
					OwnerPluginId = pluginId,
					SettingGroupId = parentGroupId,
					SettingOwner = (int)SettingOwner.Project,
					SettingType = (int)settingDefinition.SettingType
				};

				_settingRepository.Insert(settingDb);
			}

			_settingRepository.Save();
		}

		private void HandleSettingGroup(
			long pluginId,
			long? parentGroupId,
			[NotNull] PluginSettingGroupDefinition groupDefinition,
			ICollection<string> settingKeys)
		{
			var groupId = HandleGroupItself(pluginId, parentGroupId, groupDefinition);

			if(groupDefinition.SettingDefinitions != null)
			{
				foreach(var pluginSetting in groupDefinition.SettingDefinitions)
				{
					HandleSetting(pluginId, groupId, pluginSetting);

					settingKeys.Add(pluginSetting.Code);

					LogInitializedSetting(pluginId, pluginSetting.Code);
				}
			}

			if(groupDefinition.SettingGroupDefinitions == null)
			{
				return;
			}

			foreach(var innerGroupDefinition in groupDefinition.SettingGroupDefinitions)
			{
				HandleSettingGroup(pluginId, groupId, innerGroupDefinition, settingKeys);
			}
		}

		private void LogInitializedSetting(long pluginId, string settingKey) => _log.Debug(
			Resources.Resources.PluginSettingProvider_Initialize_SettingInitialized.FormatWith(
						pluginId,
						settingKey));
	}
}