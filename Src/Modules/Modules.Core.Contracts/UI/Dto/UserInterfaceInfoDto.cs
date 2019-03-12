namespace Modules.Core.Contracts.UI.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "UserInterfaceInfo")]
	public sealed class UserInterfaceInfoDto
	{
		[DataMember]
		public string Host { get; set; }

		[DataMember]
		public string Version { get; set; }
	}
}
