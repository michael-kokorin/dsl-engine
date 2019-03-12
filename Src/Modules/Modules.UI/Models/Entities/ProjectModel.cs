namespace Modules.UI.Models.Entities
{
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Resources;

	public sealed class ProjectModel
	{
		public string Alias { get; set; }

		public bool CommitToIt { get; set; }

		public bool CommitToVcs { get; set; }

		[Display(Name = @"DefaultBranchName")]
		public string DefaultBranchName { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectModel_Description_Description")]
		public string Description { get; set; }

		public long Id { get; set; }

		public long? ItPluginId { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectModel_Name_Name")]
		public string Name { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectModel_SdlPolicyStatus_SDL_Policy")]
		public SdlPolicyStatusModel SdlPolicyStatus { get; set; }

		public long? VcsPluginId { get; set; }

		public bool VcsSyncEnabled { get; set; }

		public bool EnablePoll { get; set; }

		public int? PollTimeout { get; set; }
	}
}