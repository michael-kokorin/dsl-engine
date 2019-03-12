namespace Modules.Core.Contracts.SA.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "IsTaskCancelledRequest")]
	public sealed class IsTaskCancelledRequestDto
	{
		[DataMember]
		public long TaskId { get; set; }
	}
}