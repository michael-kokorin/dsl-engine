namespace Infrastructure.Plugins
{
	using Infrastructure.Plugins.Contracts;

	/// <summary>
	///     Represents plugin setting.
	/// </summary>
	/// <seealso cref="Infrastructure.Plugins.ProjectPluginSetting"/>
	public sealed class PluginSetting: ProjectPluginSetting
	{
		/// <summary>
		///     Gets or sets the description.
		/// </summary>
		/// <value>
		///     The description.
		/// </value>
		public string Description { get; set; }

		/// <summary>
		///     Gets or sets the display name.
		/// </summary>
		/// <value>
		///     The display name.
		/// </value>
		public string DisplayName { get; set; }

		/// <summary>
		///     Gets or sets the plugin identifier.
		/// </summary>
		/// <value>
		///     The plugin identifier.
		/// </value>
		public long PluginId { get; set; }

		/// <summary>
		///     Gets or sets the type of the value.
		/// </summary>
		/// <value>
		///     The type of the value.
		/// </value>
		public PluginSettingValueType ValueType { get; set; }
	}
}