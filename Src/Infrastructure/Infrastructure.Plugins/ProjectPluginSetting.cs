namespace Infrastructure.Plugins
{
	/// <summary>
	///     Represents project plugin setting.
	/// </summary>
	public class ProjectPluginSetting
	{
		/// <summary>
		///     Gets or sets the project identifier.
		/// </summary>
		/// <value>
		///     The project identifier.
		/// </value>
		public long ProjectId { get; set; }

		/// <summary>
		///     Gets or sets the setting identifier.
		/// </summary>
		/// <value>
		///     The setting identifier.
		/// </value>
		public long SettingId { get; set; }

		/// <summary>
		///     Gets or sets the value.
		/// </summary>
		/// <value>
		///     The value.
		/// </value>
		public string Value { get; set; }
	}
}