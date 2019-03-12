namespace Modules.Core.Contracts.UI.Dto.ProjectSettings
{
	using System.Runtime.Serialization;

	[DataContract(Name = "UpdateProjectPluginSetting")]
	public sealed class UpdateProjectPluginSettingDto
	{
		[DataMember]
		public long ProjectId { get; set; }

		[DataMember]
		public long SettingId { get; set; }

		[DataMember]
		public string Value { get; set; }
	}
}