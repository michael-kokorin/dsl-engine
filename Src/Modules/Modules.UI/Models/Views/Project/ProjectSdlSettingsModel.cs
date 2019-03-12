namespace Modules.UI.Models.Views.Project
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using Modules.UI.Resources;

    public sealed class ProjectSdlSettingsModel : ProjectSettingsModelBase
    {
        [Display(ResourceType = typeof(Resources), Name = "ProjectSdlSettingsModel_SdlRules_SDL_rules")]
        public IList<SelectListItem> SdlRules { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "ProjectSdlSettingsModel_Query_Query")]
        public string Query { get; set; }

        public ProjectSdlSettingsModel()
        {
            SdlRules = new List<SelectListItem>();
        }
    }
}