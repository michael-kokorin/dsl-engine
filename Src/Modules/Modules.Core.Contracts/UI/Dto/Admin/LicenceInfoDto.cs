namespace Modules.Core.Contracts.UI.Dto.Admin
{
	using System.Runtime.Serialization;

	[DataContract(Name = "SystemLicenceInfo")]
	public sealed class LicenceInfoDto
	{
		[DataMember]
		public string Id { get; set; }

		[DataMember]
		public string Description { get; set; }
	}
}