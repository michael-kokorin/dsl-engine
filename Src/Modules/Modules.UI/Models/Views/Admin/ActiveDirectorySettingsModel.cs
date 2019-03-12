namespace Modules.UI.Models.Views.Admin
{
    using System.ComponentModel.DataAnnotations;

    using Modules.UI.Resources;

    public sealed class ActiveDirectorySettingsModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ActiveDirectorySettingsModel_RootGroupPath_You_must_specify_Active_Directory_parent_group")]
        [Display(ResourceType = typeof(Resources), Name = "ActiveDirectorySettingsModel_RootGroupPath_Root_group", Description = "ActiveDirectorySettingsModel_RootGroupPath_")]
        public string RootGroupPath { get; set; }
    }
}