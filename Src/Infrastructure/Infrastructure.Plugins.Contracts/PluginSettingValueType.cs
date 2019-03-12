namespace Infrastructure.Plugins.Contracts
{
	/// <summary>
	///     Plugin setting value visualization type
	/// </summary>
	public enum PluginSettingValueType
	{
		/// <summary>
		///     The text
		/// </summary>
		Text = 0,

		/// <summary>
		///     The password. Setting value is not visible.
		/// </summary>
		Password = 1,

		/// <summary>
		///     Setting with possible bool value
		/// </summary>
		Bool = 2
	}
}