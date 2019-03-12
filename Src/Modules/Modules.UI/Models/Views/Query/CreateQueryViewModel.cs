namespace Modules.UI.Models.Views.Query
{
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Resources;

	public sealed class CreateQueryViewModel
	{
		[Display(ResourceType = typeof(Resources), Name = "CreateQueryViewModel_ProjectId_Project")]
		public long ProjectId { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "CreateQueryViewModel_Name_Query_name")]
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }
	}
}