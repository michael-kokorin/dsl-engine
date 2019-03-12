namespace Modules.UI.Models.Views.Task
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Web.Mvc;

	using Modules.UI.Resources;

	public sealed class CreateTaskViewModel
	{
		public IEnumerable<SelectListItem> Branches { get; set; }

		[Required]
		[Display(ResourceType = typeof(Resources), Name = "CreateTaskViewModel_Repository_Repository")]
		public string Repository { get; set; }

		public long ProjectId { get; set; }
	}
}