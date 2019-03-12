namespace Modules.Core.Contracts.SA.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "ScanAgentInfo")]
	public sealed class ScanAgentInfoDto
	{
		[DataMember]
		public string AssemblyVersion { get; set; }

		[DataMember]
		public string Uid { get; set; }

		[DataMember]
		public string Version { get; set; }

		[DataMember]
		public string MachineName { get; set; }
	}
}