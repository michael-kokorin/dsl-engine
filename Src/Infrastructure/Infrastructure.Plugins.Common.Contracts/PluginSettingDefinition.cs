namespace Infrastructure.Plugins.Common.Contracts
{
	public sealed class PluginSettingDefinition
	{
		public string DisplayName { get; set; }

		public bool IsAuthentication { get; set; }

		public string Code { get; set; }

		public SettingType SettingType { get; set; }

		public SettingOwner SettingOwner { get; set; }

		public string DefaultValue { get; set; }
	}
}