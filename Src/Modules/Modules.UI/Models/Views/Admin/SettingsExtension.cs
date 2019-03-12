namespace Modules.UI.Models.Views.Admin
{
	using Modules.Core.Contracts.UI.Dto.Admin;
	using Modules.UI.Models.Data;

	internal static class SettingsExtension
	{
		public static SettingsDto ToDto(this SettingsModel settings) =>
			new SettingsDto
			{
				ActiveDirectorySettings = settings.ActiveDirectorySettings.ToDto(),
				DatabaseSettings = settings.SummarySettings.ToDto(),
				FileStorageSettings = settings.FileStorageSettings.ToDto(),
				NotificationSettings = settings.NotificationSettings.ToDto()
			};

		private static ActiveDirectorySettingsDto ToDto(this ActiveDirectorySettingsModel settings) =>
			new ActiveDirectorySettingsDto
			{
				RootGroupPath = settings.RootGroupPath
			};

		private static DatabaseSettingsDto ToDto(this SummarySettingsModel settings) =>
			new DatabaseSettingsDto
			{
				ConnectionString = settings.ConnectionString
			};

		private static FileStorageSettingsDto ToDto(this FileStorageSettingsModel settings) =>
			new FileStorageSettingsDto
			{
				TempDirPath = settings.TempDirPath
			};

		public static SettingsModel ToModel(this SettingsDto settings)
		{
			if (settings == null)
				return null;

			return new SettingsModel
			{
				ActiveDirectorySettings = settings.ActiveDirectorySettings.Tomodel(),
				SummarySettings = settings.DatabaseSettings.ToModel(),
				FileStorageSettings = settings.FileStorageSettings.ToModel(),
				ScanAgentSettings = settings.ScanAgentSettings.ToModel(),
				PluginSettings = settings.PluginSettings.ToModel(),
				NotificationSettings = settings.NotificationSettings.ToModel()
			};
		}

		private static ActiveDirectorySettingsModel Tomodel(this ActiveDirectorySettingsDto settings)
		{
			if (settings == null)
				return null;

			return new ActiveDirectorySettingsModel
			{
				RootGroupPath = settings.RootGroupPath
			};
		}

		private static SummarySettingsModel ToModel(this DatabaseSettingsDto settings)
		{
			if (settings == null)
				return null;

			return new SummarySettingsModel
			{
				ConnectionString = settings.ConnectionString
			};
		}

		private static FileStorageSettingsModel ToModel(this FileStorageSettingsDto settings)
		{
			if (settings == null)
				return null;

			return new FileStorageSettingsModel
			{
				TempDirPath = settings.TempDirPath
			};
		}

		private static ScanAgentSettingsModel ToModel(this ScanAgentSettingsDto settings)
		{
			if (settings == null)
				return null;

			return new ScanAgentSettingsModel
			{
				ScanAgents = settings.ScanAgents.ToModel()
			};
		}

		private static PluginSettingsModel ToModel(this PluginSettingsDto settings)
		{
			if (settings == null)
				return null;

			return new PluginSettingsModel
			{
				Plugins = settings.Plugins.ToModel()
			};
		}

		private static NotificationSettingsModel ToModel(this NotificationSettingsDto setting)
		{
			if (setting == null)
				return null;

			return new NotificationSettingsModel
			{
				MainServerPort = setting.MainServerPort,
				IsSslEnabled = setting.IsSslEnabled,
				MailBox = setting.MailBox,
				MailServerHost = setting.MailServerHost,
				Password = setting.Password,
				UserName = setting.UserName
			};
		}

		private static NotificationSettingsDto ToDto(this NotificationSettingsModel model)
		{
			if (model == null)
				return null;

			return new NotificationSettingsDto
			{
				IsSslEnabled = model.IsSslEnabled,
				MailBox = model.MailBox,
				MailServerHost = model.MailServerHost,
				MainServerPort = model.MainServerPort,
				Password = model.Password,
				UserName = model.UserName
			};
		}
	}
}