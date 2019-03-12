namespace Modules.Core.Contracts.Query.Dto
{
	using System.Runtime.Serialization;

	[DataContract (Name="DataSource")]
	public sealed class DataSourceDto
	{
		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public string Key { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public long Id { get; set; }
	}
}