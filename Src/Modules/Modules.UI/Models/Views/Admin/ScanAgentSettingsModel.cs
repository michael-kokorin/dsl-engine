namespace Modules.UI.Models.Views.Admin
{
    using System.ComponentModel.DataAnnotations;

    using Modules.UI.Models.Data;
    using Modules.UI.Resources;

    public sealed class ScanAgentSettingsModel
    {
        [Display(ResourceType = typeof(Resources), Name = "ScanAgentSettingsModel_ScanAgents_Scan_agents")]
        public TableModel ScanAgents { get; set; }
    }
}