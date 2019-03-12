namespace Modules.UI.Models.Views.PersonalCabinet
{
	using Modules.Core.Contracts.UI.Dto.UserSettings;
	using Modules.UI.Models.Entities;

	internal static class ModelExtension
	{
		public static PluginModel ToModel(this PluginDto plugin)
		{
			if (plugin == null)
				return null;

			return new PluginModel
			{
				Id = plugin.Id,
				DisplayName = plugin.DisplayName
			};
		}

		public static PluginSettingModel ToModel(this PluginSettingDto pluginSetting)
		{
			if (pluginSetting == null)
				return null;

			return new PluginSettingModel
			{
				Description = pluginSetting.Description,
				DisplayName = pluginSetting.DisplayName,
				SettingId = pluginSetting.SettingId,
				Type = pluginSetting.ValueType.ToModel(),
				Value = pluginSetting.Value
			};
		}

		private static PluginSettingTypeModel ToModel(this PluginSettingValueTypeDto pluginSettingType)
		{
			switch (pluginSettingType)
			{
				case PluginSettingValueTypeDto.Password:
					return PluginSettingTypeModel.Password;
				case PluginSettingValueTypeDto.Text:
					return PluginSettingTypeModel.Text;
				case PluginSettingValueTypeDto.Bool:
					return PluginSettingTypeModel.Bool;
				default:
					return PluginSettingTypeModel.Unknown;
			}
		}
	}
}