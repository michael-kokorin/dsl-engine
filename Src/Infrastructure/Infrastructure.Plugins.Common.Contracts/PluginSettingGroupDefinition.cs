namespace Infrastructure.Plugins.Common.Contracts
{
	using System.Collections.Generic;

	public sealed class PluginSettingGroupDefinition
	{
		public string DisplayName { get; set; }

		public string Code { get; set; }

		public List<PluginSettingDefinition> SettingDefinitions { get; set; }

		public List<PluginSettingGroupDefinition> SettingGroupDefinitions { get; set; }
	}
}