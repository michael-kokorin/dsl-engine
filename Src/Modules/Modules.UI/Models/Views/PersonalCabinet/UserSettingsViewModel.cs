namespace Modules.UI.Models.Views.PersonalCabinet
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Models.Entities;
	using Modules.UI.Resources;

	public sealed class UserSettingsViewModel
	{
		[Display(ResourceType = typeof(Resources), Name = "UserSettingsViewModel_UserInfo_User_info")]
		public UserModel UserInfo { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "UserSettingsModel_ItPluginSettings_Issue_Tracker")]
		public IList<ProjectPluginViewModel> ItPluginSettings { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "UserSettingsModel_VcsPluginSettings_Version_control")]
		public IList<ProjectPluginViewModel> VcsPluginSettings { get; set; }

		public UserSettingsViewModel()
		{
			UserInfo = new UserModel();

			ItPluginSettings = new List<ProjectPluginViewModel>();

			VcsPluginSettings = new List<ProjectPluginViewModel>();
		}
	}
}