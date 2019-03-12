namespace Modules.UI.Models.Views.Task
{
    using System.ComponentModel.DataAnnotations;

    using Modules.UI.Models.Data;
    using Modules.UI.Models.Entities;
    using Modules.UI.Resources;

    public sealed class TaskResultsViewModel
    {
        public bool IsCanBeStopped { get; set; }

        public TaskModel Task { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TaskModel_ScanCore_Scan_core")]
        public ScanCoreModel ScanCore { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TaskResultsViewModel_VcsPlugin_VCS_plugin")]
        public PluginModel VcsPlugin { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TaskResultsViewModel_ItPlugin_IT_plugin")]
        public PluginModel ItPlugin { get; set; }

        public TableModel Table { get; set; }
    }
}