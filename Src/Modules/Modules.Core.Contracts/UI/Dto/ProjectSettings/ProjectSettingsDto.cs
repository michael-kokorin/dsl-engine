namespace Modules.Core.Contracts.UI.Dto.ProjectSettings
{
	using System.Runtime.Serialization;

	[DataContract(Name = "ProjectSettings")]
	public sealed class ProjectSettingsDto
	{
		[DataMember]
		public string Alias { get; set; }

		[DataMember]
		public bool CommitToIt { get; set; }

		[DataMember]
		public bool CommitToVcs { get; set; }

		[DataMember]
		public string DefaultBranchName { get; set; }

		[DataMember]
		public string DisplayName { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public bool VcsSyncEnabled { get; set; }

		[DataMember]
		public bool EnablePoll { get; set; }

		[DataMember]
		public int? PollTimeout { get; set; }
	}
}