namespace Modules.UI.Models.Views.Plugin
{
	using System.Collections.Generic;

	using Modules.UI.Models.Entities;

	public sealed class PluginSettingsModel
	{
		public long ProjectId { get; set; }

		public long PluginId { get; set; }

		public IList<PluginSettingModel> Settings { get; set; }
	}
}