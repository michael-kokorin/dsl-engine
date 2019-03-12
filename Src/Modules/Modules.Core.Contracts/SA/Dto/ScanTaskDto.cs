namespace Modules.Core.Contracts.SA.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "ScanTask")]
	public sealed class ScanTaskDto
	{
		[DataMember(IsRequired = true)]
		public long Id { get; set; }

		[DataMember(IsRequired = true)]
		public string Path { get; set; }

		[DataMember]
		public ScanTaskCoreDto[] Cores { get; set; }
	}
}