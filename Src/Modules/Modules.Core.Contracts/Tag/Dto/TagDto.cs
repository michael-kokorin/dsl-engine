namespace Modules.Core.Contracts.Tag.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "Tag")]
	public sealed class TagDto
	{
		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public string Name { get; set; }
	}
}