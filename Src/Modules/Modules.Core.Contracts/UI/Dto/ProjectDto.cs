namespace Modules.Core.Contracts.UI.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "Project")]
	public sealed class ProjectDto
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
		public string Description { get; set; }

		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public long? VcsPluginId { get; set; }

		[DataMember]
		public bool VcsSyncEnabled { get; set; }

		[DataMember]
		public long? ItPluginId { get; set; }

		[DataMember]
		public SdlPolicyStatusDto SdlPolicyStatus { get; set; }

		[DataMember]
		public bool EnablePoll { get; set; }

		[DataMember]
		public int? PollTimeout { get; set; }
	}
}