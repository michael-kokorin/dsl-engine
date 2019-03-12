namespace Modules.Core.Contracts.UI.Dto.ProjectSettings
{
	using System.Runtime.Serialization;

	[DataContract(Name = "UpdateProjectPluginSettings")]
	public sealed class UpdateProjectPluginSettingsDto
	{
		[DataMember]
		public UpdateProjectPluginSettingDto[] Settings { get; set; }
	}
}