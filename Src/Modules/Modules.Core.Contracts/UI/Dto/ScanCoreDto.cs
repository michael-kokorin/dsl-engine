namespace Modules.Core.Contracts.UI.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "ScanCore")]
	public sealed class ScanCoreDto
	{
		[DataMember]
		public string DisplayName { get; set; }

		[DataMember]
		public string Key { get; set; }
	}
}