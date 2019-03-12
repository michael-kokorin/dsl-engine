namespace Modules.Core.Contracts.Report.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "Report")]
	public sealed class ReportDto
	{
		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public long? Id { get; set; }

		[DataMember]
		public long? ProjectId { get; set; }

		[DataMember]
		public string Rule { get; set; }
	}
}