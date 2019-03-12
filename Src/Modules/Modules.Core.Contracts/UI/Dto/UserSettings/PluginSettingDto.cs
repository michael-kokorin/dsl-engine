namespace Modules.Core.Contracts.UI.Dto.UserSettings
{
	using System.Runtime.Serialization;

	[DataContract(Name = "PluginSetting")]
	public sealed class PluginSettingDto
	{
		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public string DisplayName { get; set; }

		[DataMember]
		public long PluginId { get; set; }

		[DataMember]
		public long ProjectId { get; set; }

		[DataMember]
		public long SettingId { get; set; }

		[DataMember]
		public PluginSettingValueTypeDto ValueType { get; set; }

		[DataMember]
		public string Value { get; set; }
	}
}