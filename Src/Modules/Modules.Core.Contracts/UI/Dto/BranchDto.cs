namespace Modules.Core.Contracts.UI.Dto
{
	using System;
	using System.Runtime.Serialization;

	[DataContract]
	public sealed class BranchDto
	{
		[DataMember]
		public string Id { get; set; }

		[DataMember]
		public bool IsDefault { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public DateTime? LastScanFinishedUtc { get; set; }
	}
}