namespace Modules.Core.Contracts.UI.Dto.Data
{
	using System.Runtime.Serialization;

	[DataContract(Name = "QueryException")]
	public sealed class QueryExceptionDto
	{
		[DataMember]
		public string Message { get; set; }
	}
}