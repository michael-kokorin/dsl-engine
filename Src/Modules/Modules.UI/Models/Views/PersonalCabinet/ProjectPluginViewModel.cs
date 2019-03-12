namespace Modules.UI.Models.Views.PersonalCabinet
{
    using System.ComponentModel.DataAnnotations;

    using Modules.UI.Models.Entities;
    using Modules.UI.Resources;

    public sealed class ProjectPluginViewModel
    {
        [Display(ResourceType = typeof(Resources), Name = "ProjectPluginsModel_Project_Project")]
        public ProjectModel Project { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "ProjectPluginsModel_Plugin_Plugin")]
        public PluginSettingsViewModel PluginSettingsView { get; set; }
    }
}