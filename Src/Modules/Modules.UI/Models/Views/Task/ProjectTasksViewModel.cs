namespace Modules.UI.Models.Views.Task
{
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Models.Data;
	using Modules.UI.Models.Entities;
	using Modules.UI.Resources;

	public sealed class ProjectTasksViewModel
	{
		public bool IsCanCreateTask { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectTasksViewModel_ItPlugin_IT_plugin")]
		public PluginModel ItPlugin { get; set; }

		public ProjectModel Project { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectModel_ScanCore_Scan_core")]
		public ScanCoreModel ScanCore { get; set; }

		public TableModel Table { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectTasksViewModel_VcsPlugin_VCS_plugin")]
		public PluginModel VcsPlugin { get; set; }
	}
}