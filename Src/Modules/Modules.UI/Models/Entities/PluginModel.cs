namespace Modules.UI.Models.Entities
{
    using System.ComponentModel.DataAnnotations;

    using Modules.UI.Resources;

    public sealed class PluginModel
    {
        public long Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "PluginSettingsModel_PluginName_Plugin_name")]
        public string DisplayName { get; set; }
    }
}