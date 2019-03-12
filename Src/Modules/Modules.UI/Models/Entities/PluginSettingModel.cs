namespace Modules.UI.Models.Entities
{
	public class PluginSettingModel
	{
		public string Description { get; set; }

		public string DisplayName { get; set; }

		public long SettingId { get; set; }

		public string Value { get; set; }

		public bool BoolValue
		{
			get
			{
				bool result;

				return bool.TryParse(Value, out result) && result;
			}
			set { Value = value.ToString(); }
		}

		public PluginSettingTypeModel Type { get; set; }
	}
}