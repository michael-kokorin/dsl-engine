namespace Modules.UI.Models.Views.PersonalCabinet
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Models.Entities;
	using Modules.UI.Resources;

	public sealed class PluginSettingsViewModel
	{
		public PluginModel Plugin { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "PluginSettingsModel_Settings_Settings")]
		public IList<PluginSettingModel> Settings { get; set; }
	}
}