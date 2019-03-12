namespace Modules.Core.Contracts.Query.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "Query")]
	public sealed class QueryDto
	{
		[DataMember]
		public string Comment { get; set; }

		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public bool IsSystem { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Model { get; set; }

		[DataMember]
		public int Privacy { get; set; }

		[DataMember]
		public long ProjectId { get; set; }

		[DataMember]
		public string Query { get; set; }

		[DataMember]
		public int Visibility { get; set; }
	}
}