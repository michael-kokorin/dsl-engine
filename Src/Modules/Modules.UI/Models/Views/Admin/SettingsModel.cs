namespace Modules.UI.Models.Views.Admin
{
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Resources;

	public sealed class SettingsModel
	{
		[ReadOnly(true)]
		[Display(ResourceType = typeof(Resources), Name = "SettingsModel_DatabaseSettings_Summary")]
		public SummarySettingsModel SummarySettings { get; set; }

		[ReadOnly(true)]
		[Display(ResourceType = typeof(Resources), Name = "SettingsModel_FileStorageSettings_File_storage")]
		public FileStorageSettingsModel FileStorageSettings { get; set; }

		[ReadOnly(true)]
		[Display(ResourceType = typeof(Resources), Name = "SettingsModel_ActiveDirectorySettings_Active_Directory")]
		public ActiveDirectorySettingsModel ActiveDirectorySettings { get; set; }

		[ReadOnly(true)]
		[Display(ResourceType = typeof(Resources), Name = "SettingsModel_ScanAgentSettings_Scan_agents")]
		public ScanAgentSettingsModel ScanAgentSettings { get; set; }

		[ReadOnly(true)]
		[Display(ResourceType = typeof(Resources), Name = "SettingsModel_PluginSettings_Plugins")]
		public PluginSettingsModel PluginSettings { get; set; }

		[ReadOnly(true)]
		[Display(ResourceType = typeof(Resources), Name = "SettingsModel_NotificationSettings_Notifications")]
		public NotificationSettingsModel NotificationSettings { get; set; }

		public SettingsModel()
		{
			SummarySettings = new SummarySettingsModel();

			FileStorageSettings = new FileStorageSettingsModel();

			ActiveDirectorySettings = new ActiveDirectorySettingsModel();

			ScanAgentSettings = new ScanAgentSettingsModel();

			PluginSettings = new PluginSettingsModel();

			NotificationSettings = new NotificationSettingsModel();
		}
	}
}