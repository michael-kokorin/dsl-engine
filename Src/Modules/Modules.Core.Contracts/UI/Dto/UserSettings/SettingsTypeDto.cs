namespace Modules.Core.Contracts.UI.Dto.UserSettings
{
	using System.Runtime.Serialization;

	[DataContract(Name = "PluginSettingValueType")]
	public enum PluginSettingValueTypeDto
	{
		[EnumMember]
		Text = 0,

		[EnumMember]
		Password = 1,

		[EnumMember]
		Bool = 2
	}
}