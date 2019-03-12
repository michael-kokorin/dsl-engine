namespace Modules.UI.Models.Views.Admin
{
    using System.ComponentModel.DataAnnotations;

    using Modules.UI.Models.Data;
    using Modules.UI.Resources;

    public sealed class PluginSettingsModel
    {
        [Display(ResourceType = typeof(Resources), Name = "PluginSettingsModel_Plugins_Plugins_list")]
        public TableModel Plugins { get; set; }
    }
}