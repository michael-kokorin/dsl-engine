namespace Modules.UI.Models.Views.Project
{
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Models.Data;
	using Modules.UI.Models.Entities;
	using Modules.UI.Resources;

	public sealed class ProjectViewModel
	{
		public bool IsCanEdit { get; set; }

		public bool IsCanViewHealthStat { get; set; }

		public bool IsCanViewMetricsStat { get; set; }

		public bool IsCanViewVulnerabilitiesStat { get; set; }

		public ProjectModel Project { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectViewModel_HealthTable_Health")]
		public TableModel HealthTable { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectViewModel_MetricsTable_Metrics")]
		public TableModel MetricsTable { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectViewModel_VulnerabilityTable_Vulnerabilities")]
		public TableModel VulnerabilityTable { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectModel_ScanCore_Scan_core")]
		public ScanCoreModel ScanCore { get; set; }

		public RoleModel[] Roles { get; set; }

		public ProjectViewModel()
		{
			HealthTable = new TableModel();

			MetricsTable = new TableModel();

			VulnerabilityTable = new TableModel();
		}
	}
}