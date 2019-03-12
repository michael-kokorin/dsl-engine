namespace Modules.Core.Contracts.SA.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "IsTaskCancelledResponse")]
	public sealed class IsTaskCancelledResponseDto
	{
		[DataMember]
		public long TaskId { get; set; }

		[DataMember]
		public bool IsCancelled { get; set; }
	}
}