namespace Modules.Core.Contracts.SA.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "GetScanTask")]
	public sealed class GetScanTaskDto
	{
		[DataMember]
		public string ScanAgentId { get; set; }
	}
}