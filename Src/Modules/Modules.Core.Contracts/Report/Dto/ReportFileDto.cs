namespace Modules.Core.Contracts.Report.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "ReportFile")]
	public sealed class ReportFileDto
	{
		[DataMember]
		public string Title { get; set; }

		[DataMember]
		public byte[] Content { get; set; }
	}
}