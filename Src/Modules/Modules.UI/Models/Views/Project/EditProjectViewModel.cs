namespace Modules.UI.Models.Views.Project
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Web.Mvc;

	using Modules.Core.Contracts.UI.Dto;
	using Modules.UI.Resources;

	public sealed class EditProjectViewModel
	{
		public SettingGroupDto[] SettingTabs { get; set; }

		public long ProjectId { get; set; }

		public long SelectedRoleId { get; set; }

		public long SelectedSdlRuleId { get; set; }

		public long SelectedNotificationId { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectVcsSettingsModel_PluginId_Plugin")]
		public long? SelectedVcsId { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectItSettingsModel_PluginId_Plugin")]
		public long? SelectedItId { get; set; }

		public IEnumerable<long> SelectedAuthorities { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "EditProjectViewModel_ProjectSettings_Project_settings")]
		public ProjectSettingsModel ProjectSettings { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "EditProjectViewModel_VcsSettings_Version_control")]
		public ProjectVcsSettingsModel VcsSettings { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "EditProjectViewModel_ItSettings_Issue_tracker")]
		public ProjectItSettingsModel ItSettings { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "EditProjectViewModel_RbacSettings_Access_control")]
		public ProjectRbacSettingsModel RbacSettings { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "EditProjectViewModel_SdlSettings_SDL_Rules")]
		public ProjectSdlSettingsModel SdlSettings { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "EditProjectViewModel_Notifications_Notifications")]
		public ProjectNotificationSettingsModel Notifications { get; set; }

		public IEnumerable<SelectListItem> VcsPlugins { get; set; }

		public IEnumerable<SelectListItem> ItPlugins { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "EditProjectViewModel_ProjectRoles_User_role")]
		public IList<SelectListItem> ProjectRoles { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "EditProjectViewModel_Authorities_User_Authorities")]
		public IList<SelectListItem> Authorities { get; set; }

		public EditProjectViewModel()
		{
			ProjectSettings = new ProjectSettingsModel();

			VcsSettings = new ProjectVcsSettingsModel();

			ItSettings = new ProjectItSettingsModel();

			RbacSettings = new ProjectRbacSettingsModel();

			SdlSettings = new ProjectSdlSettingsModel();

			ProjectRoles = new List<SelectListItem>();

			Authorities = new List<SelectListItem>();

			Notifications = new ProjectNotificationSettingsModel();
		}
	}
}