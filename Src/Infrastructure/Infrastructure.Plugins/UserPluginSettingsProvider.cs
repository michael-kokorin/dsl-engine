namespace Infrastructure.Plugins
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class UserPluginSettingsProvider: IUserPluginSettingsProvider
	{
		private readonly ISettingValuesRepository _settingValuesRepository;

		private readonly ISettingRepository _settingRepository;

		public UserPluginSettingsProvider(
			[NotNull] ISettingValuesRepository settingValuesRepository,
			[NotNull] ISettingRepository settingRepository)
		{
			if (settingValuesRepository == null)
			{
				throw new ArgumentNullException(nameof(settingValuesRepository));
			}
			if (settingRepository == null)
			{
				throw new ArgumentNullException(nameof(settingRepository));
			}

			_settingValuesRepository = settingValuesRepository;
			_settingRepository = settingRepository;
		}

		public IEnumerable<PluginSetting> GetSettings(long userId, long projectId, long pluginId)
		{
			var settings = _settingValuesRepository.Query()
									.Where(
										item =>
										(item.Settings.OwnerPluginId == pluginId) && (item.Settings.SettingOwner == (int)SettingOwner.User) &&
										(item.EntityId == userId) && (item.ProjectId == projectId) && !item.Settings.IsArchived)
									.ToDictionary(item => item.Settings.Code, item => item);
			var existingSettings =
				_settingRepository.Query()
								.Where(item => (item.OwnerPluginId == pluginId) && (item.SettingOwner == (int)SettingOwner.User) && !item.IsArchived)
								.ToList();
			foreach (var existingSetting in existingSettings)
			{
				if (settings.ContainsKey(existingSetting.Code))
				{
					continue;
				}

				var setting = new SettingValues
				{
					EntityId = userId,
					SettingId = existingSetting.Id,
					Value = existingSetting.DefaultValue,
					Settings = existingSetting,
					ProjectId = projectId
				};
				_settingValuesRepository.Insert(setting);
				settings.Add(existingSetting.Code, setting);
			}

			_settingValuesRepository.Save();

			return settings.Values.Select(
				item => new PluginSetting
				{
					DisplayName = item.Settings.DisplayName,
					Value = item.Value,
					Description = item.Settings.DisplayName,
					PluginId = pluginId,
					SettingId = item.Id,
					ProjectId = projectId,
					ValueType =
								item.Settings.SettingType == (int)SettingType.Boolean
									? PluginSettingValueType.Bool
									: (item.Settings.SettingType == (int)SettingType.Password
											? PluginSettingValueType.Password
											: PluginSettingValueType.Text)
				});
		}

		public void SetValues(long userId, IEnumerable<ProjectPluginSetting> pluginSettingValues)
		{
			if (pluginSettingValues == null)
			{
				throw new ArgumentNullException(nameof(pluginSettingValues));
			}

			foreach (var settingValue in pluginSettingValues)
			{
				var setting = _settingValuesRepository.Query().FirstOrDefault(item => item.Id == settingValue.SettingId);
				if (setting == null)
				{
					throw new Exception();
				}

				setting.Value = settingValue.Value?.Trim();
			}

			_settingValuesRepository.Save();
		}
	}
}