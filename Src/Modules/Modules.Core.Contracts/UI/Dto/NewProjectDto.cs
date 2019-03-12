namespace Modules.Core.Contracts.UI.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "NewProject")]
	public sealed class NewProjectDto
	{
		[DataMember(IsRequired = true)]
		public string Alias { get; set; }

		[DataMember(IsRequired = true)]
		public string DefaultBranchName { get; set; }

		[DataMember(IsRequired = true)]
		public string Name { get; set; }
	}
}