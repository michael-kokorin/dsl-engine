namespace Modules.Core.Contracts.UI.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "TaskStatus")]
	public enum TaskStatusDto
	{
		[EnumMember]
		New = 0,

		[EnumMember]
		PreProcessing = 1,

		[EnumMember]
		ReadyToStan = 2,

		[EnumMember]
		Scanning = 3,

		[EnumMember]
		ReadyToPostProcessing = 5,

		[EnumMember]
		PostProcessing = 6,

		[EnumMember]
		Done = 7
	}
}