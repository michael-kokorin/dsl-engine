using System.Runtime.Serialization;

namespace Modules.Core.Contracts.SA.Dto
{
	[DataContract(Name = "ScanTaskCore")]
	public sealed class ScanTaskCoreDto
	{
		[DataMember]
		public string Core { get; set; }

		[DataMember]
		public string CodeLocation { get; set; }

		[DataMember]
		public string CoreParameters { get; set; }
	}
}