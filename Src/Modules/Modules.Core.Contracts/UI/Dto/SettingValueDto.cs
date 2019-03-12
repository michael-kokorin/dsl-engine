namespace Modules.Core.Contracts.UI.Dto
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;

	using Newtonsoft.Json;

	[DataContract]
	public sealed class SettingValueDto
	{
		[DataMember]
		public List<SettingGroupDto> ChildGroups { get; set; }

		[DataMember]
		public string DefaultValue { get; set; }

		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public string Title { get; set; }

		[DataMember]
		public SettingValueTypeDto Type { get; set; }

		[DataMember]
		public string Value { get; set; }

		public IEnumerable<SettingSubitem> GetSubitems() =>
			Type != SettingValueTypeDto.List
				? new[]
					{
						new SettingSubitem
						{
							Key = string.Empty,
							Text = Value
						}
					}
				: JsonConvert.DeserializeObject<SettingSubitem[]>(DefaultValue);

		public SettingSubitem GetCurrentSubitem()
		{
			if(Type != SettingValueTypeDto.List)
				return new SettingSubitem
							{
								Key = string.Empty,
								Text = Value
							};

			var items = GetSubitems();
			return items.FirstOrDefault(_ => _.Key == Value) ?? items.FirstOrDefault();
		}
	}
}