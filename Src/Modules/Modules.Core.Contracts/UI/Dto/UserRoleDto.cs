namespace Modules.Core.Contracts.UI.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "UserRole")]
	public sealed class UserRoleDto
	{
		[DataMember]
		public string DisplayName { get; set; }

		[DataMember]
		public string GroupName { get; set; }

		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public long? ProjectId { get; set; }

		[DataMember]
		public string Sid { get; set; }
	}
}