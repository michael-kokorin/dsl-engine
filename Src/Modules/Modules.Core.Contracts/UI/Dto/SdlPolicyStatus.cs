namespace Modules.Core.Contracts.UI.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "SdlPolicyStatus")]
	public enum SdlPolicyStatusDto
	{
		[EnumMember]
		Unknown = 0,

		[EnumMember]
		Success = 1,

		[EnumMember]
		Failed = 2,

		[EnumMember]
		Error = 3
	}
}