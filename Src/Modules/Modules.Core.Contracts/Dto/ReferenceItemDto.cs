using System.Runtime.Serialization;

namespace Modules.Core.Contracts.Dto
{
	[DataContract(Name = "ReferenceItem")]
	public sealed class ReferenceItemDto
	{
		[DataMember]
		public string Text { get; set; }

		[DataMember]
		public long Value { get; set; }
	}
}