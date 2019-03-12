namespace Modules.UI.Models.Views.Project
{
	using System.ComponentModel.DataAnnotations;
	using System.Web.Mvc;

	using Modules.UI.Resources;

	public sealed class ProjectSettingsModel : ProjectSettingsModelBase
	{
		[Required(AllowEmptyStrings = false)]
		[StringLength(32, MinimumLength = 4)]
		[RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceType = typeof(Resources),
			ErrorMessageResourceName = "NewProjectViewModel_Alias_Digits_and_characters_only")]
		[Display(ResourceType = typeof(Resources), Name = "ProjectSettingsModel_Alias_Alias",
			Description = "ProjectSettingsModel_Alias_Only_characters_and_numbers")]
		public string Alias { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectSettingsModel_CommitToIt_Create_issues")]
		public bool CommitToIt { get; set; }

		public bool CommitToItEnabled { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectSettingsModel_CommitToVcs_Commit_vulnerabilities")]
		public bool CommitToVcs { get; set; }

		public bool CommitToVcsEnabled { get; set; }

		[Required(AllowEmptyStrings = false)]
		[Display(ResourceType = typeof(Resources), Name = "ProjectSettingsModel_DefaultBranchName_Default_branch_name")]
		public string DefaultBranchName { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(32, MinimumLength = 4)]
		[Display(ResourceType = typeof(Resources), Name = "ProjectSettingsModel_DisplayName_Display_name")]
		public string DisplayName { get; set; }

		[AllowHtml]
		[Display(ResourceType = typeof(Resources), Name = "ProjectSettingsModel_Description_Description")]
		public string Description { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectSettingsModel_VcsSyncEnabled_VCS_sync_enabled")]
		public bool VcsSyncEnabled { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectSettingsModel_EnablePoll")]
		public bool EnablePoll { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectSettingsModel_PollTimeout")]
		public string PollTimeout { get; set; }
	}
}