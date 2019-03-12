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
	internal sealed class ProjectPluginSettingsProvider: IProjectPluginSettingsProvider
	{
		private readonly ISettingValuesRepository _settingValuesRepository;

		private readonly ISettingRepository _settingRepository;

		public ProjectPluginSettingsProvider(
			[NotNull] ISettingValuesRepository settingValuesRepository,
			[NotNull] ISettingRepository settingRepository)
		{
			if(settingValuesRepository == null)
			{
				throw new ArgumentNullException(nameof(settingValuesRepository));
			}
			if(settingRepository == null)
			{
				throw new ArgumentNullException(nameof(settingRepository));
			}

			_settingValuesRepository = settingValuesRepository;
			_settingRepository = settingRepository;
		}

		public IEnumerable<PluginSetting> GetSettings(long projectId, long pluginid)
		{
			var settings = _settingValuesRepository.Query()
									.Where(
										item =>
										(item.Settings.OwnerPluginId == pluginid) && (item.Settings.SettingOwner == (int)SettingOwner.Project) &&
										(item.EntityId == projectId) && !item.Settings.IsArchived)
									.ToDictionary(item => item.Settings.Code, item => item);
			var existingSettings =
				_settingRepository.Query()
								.Where(
									item =>
											(item.OwnerPluginId == pluginid) && (item.SettingOwner == (int)SettingOwner.Project) && !item.IsArchived)
								.ToList();
			foreach(var existingSetting in existingSettings)
			{
				if(settings.ContainsKey(existingSetting.Code))
				{
					continue;
				}

				var setting = new SettingValues
							{
								EntityId = projectId,
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
							PluginId = pluginid,
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

		public void SetValues(long projectId, IEnumerable<ProjectPluginSetting> values)
		{
			if(values == null)
			{
				throw new ArgumentNullException(nameof(values));
			}

			foreach(var settingValue in values)
			{
				var setting = _settingValuesRepository.Query().FirstOrDefault(item => item.Id == settingValue.SettingId);
				if(setting == null)
				{
					throw new Exception();
				}

				setting.Value = settingValue.Value?.Trim();
			}

			_settingValuesRepository.Save();
		}
	}
}