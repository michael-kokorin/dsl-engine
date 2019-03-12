namespace Modules.Core.Contracts.SA.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "ScanAgentSettings")]
	public sealed class ScanAgentSettingsDto
	{
		[DataMember(IsRequired = true)]
		public string ScanAgentId { get; set; }

		[DataMember(IsRequired = true)]
		public bool IsCompatible { get; set; }
	}
}