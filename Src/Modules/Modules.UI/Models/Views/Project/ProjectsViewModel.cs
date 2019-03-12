namespace Modules.UI.Models.Views.Project
{
    using System.Collections.Generic;

    public sealed class ProjectsViewModel
    {
        public IList<ProjectViewModel> Projects { get; set; }

        public bool IsCanCreateProject { get; set; }
    }
}