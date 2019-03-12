namespace Modules.UI.Models.Views.Report
{
    using Modules.UI.Models.Data;
    using Modules.UI.Models.Entities;

    public sealed class ReportViewModel
    {
        public ReportModel Report { get; set; }

        public TableModel Table { get; set; }
    }
}