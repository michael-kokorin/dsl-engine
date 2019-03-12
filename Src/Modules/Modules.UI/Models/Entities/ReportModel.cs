namespace Modules.UI.Models.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Modules.UI.Resources;

    public sealed class ReportModel
    {
        [Display(ResourceType = typeof(Resources), Name = "ReportModel_Description_Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "ReportModel_Name_Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "ReportModel_Created_Created")]
        public DateTime Created { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "ReportModel_CreatedBy_Created_by")]
        public string CreatedBy { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "ReportModel_Id_Id")]
        public long Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "ReportModel_Modified_Modified")]
        public DateTime Modified { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "ReportModel_ModifiedBy_Modified_by")]
        public string ModifiedBy { get; set; }

        public long? ProjectId { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "ReportModel_ProjectName_Project")]
        public string ProjectName { get; set; }
    }
}