namespace Modules.UI.Models.Views.Project
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Web.Mvc;

	using Modules.UI.Resources;

	public sealed class ProjectNotificationSettingsModel : ProjectSettingsModelBase
	{
		[Display(ResourceType = typeof(Resources), Name = "ProjectNotificationSettingsModel_NotificationRules_Notifications")]
		public IList<SelectListItem> NotificationRules { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectNotificationSettingsModel_Query_Query")]
		public string Query { get; set; }

		public ProjectNotificationSettingsModel()
		{
			NotificationRules = new List<SelectListItem>();
		}
	}
}