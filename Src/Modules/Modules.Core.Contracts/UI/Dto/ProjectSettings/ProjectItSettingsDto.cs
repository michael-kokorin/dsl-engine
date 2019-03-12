namespace Modules.Core.Contracts.UI.Dto.ProjectSettings
{
	using System.Runtime.Serialization;

	[DataContract(Name = "ProjectItSettings")]
	public sealed class ProjectItSettingsDto
	{
		[DataMember]
		public long PluginId { get; set; }

		[DataMember]
		public UpdateProjectPluginSettingDto[] Settings { get; set; }
	}
}