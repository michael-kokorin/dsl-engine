namespace Modules.UI.Models.Views.Admin
{
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Resources;

	public sealed class SummarySettingsModel
	{
		[Required(AllowEmptyStrings = false)]
		[Display(ResourceType = typeof(Resources), Name = "DatabaseSettingsModel_ConnectionString_Connection_string",
			Description = "DatabaseSettingsModel_ConnectionString_Connection_string_to_database")]
		public string ConnectionString { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "SummarySettingsModel_LicenceId_Licence_Id")]
		public string LicenceId { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "SummarySettingsModel_LicenceDescription_Licence_description")]
		public string LicenceDescription { get; set; }
	}
}