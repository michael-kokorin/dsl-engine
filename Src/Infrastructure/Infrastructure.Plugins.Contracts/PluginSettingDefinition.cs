namespace Infrastructure.Plugins.Contracts
{
	/// <summary>
	///     Plugin setting definition
	/// </summary>
	public sealed class PluginSettingDefinition
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
		///     Gets or sets the level.
		/// </summary>
		/// <value>
		///     The level.
		/// </value>
		public PluginSettingLevel Level { get; set; }

		/// <summary>
		///     Gets or sets the type of the value.
		/// </summary>
		/// <value>
		///     The type of the value.
		/// </value>
		public PluginSettingValueType ValueType { get; set; }
	}
}