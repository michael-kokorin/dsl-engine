namespace Modules.UI.Models.Views.Project
{
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Resources;

	public sealed class NewProjectViewModel
	{
		[Required(AllowEmptyStrings = false)]
		[StringLength(32, MinimumLength = 4)]
		[RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceType = typeof(Resources),
			ErrorMessageResourceName = "NewProjectViewModel_Alias_Digits_and_characters_only")]
		[Display(ResourceType = typeof(Resources), Name = "NewProjectViewModel_Alias_Alias")]
		public string Alias { get; set; }

		[Required(AllowEmptyStrings = false)]
		[Display(ResourceType = typeof(Resources), Name = "NewProjectViewModel_DefaultBranchName_Default_branch")]
		public string DefaultBranchName { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(32, MinimumLength = 4)]
		[Display(ResourceType = typeof(Resources), Name = "NewProjectViewModel_DisplayName_Display_name")]
		public string DisplayName { get; set; }

		// ReSharper disable once MemberCanBeInternal
		public NewProjectViewModel()
		{
			DefaultBranchName = "master";
		}
	}
}