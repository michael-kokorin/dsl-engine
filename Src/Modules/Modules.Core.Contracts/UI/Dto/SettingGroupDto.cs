namespace Modules.Core.Contracts.UI.Dto
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	[DataContract]
	public sealed class SettingGroupDto
	{
		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public string Title { get; set; }

		[DataMember]
		public List<SettingValueDto> Values { get; set; }

		[DataMember]
		public List<SettingGroupDto> Groups { get; set; }
	}
}