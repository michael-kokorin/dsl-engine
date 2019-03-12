namespace Modules.Core.Contracts.UI.Dto.Data
{
	using System.Runtime.Serialization;

	[DataContract(Name = "Table")]
	public sealed class TableDto
	{
		[DataMember]
		public TableColumnDto[] Columns { get; set; }

		[DataMember]
		public QueryExceptionDto[] Exceptions { get; set; }

		[DataMember]
		public TableRowDto[] Rows { get; set; }

		public TableDto()
		{
			Columns = new TableColumnDto[0];

			Exceptions = new QueryExceptionDto[0];

			Rows = new TableRowDto[0];
		}
	}
}