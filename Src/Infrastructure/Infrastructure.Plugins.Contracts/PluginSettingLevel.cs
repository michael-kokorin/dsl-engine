namespace Infrastructure.Plugins.Contracts
{
	/// <summary>
	///     Level of project settings (Project/User)
	/// </summary>
	public enum PluginSettingLevel
	{
		/// <summary>
		///     The project level. Defines in project settings.
		/// </summary>
		Project = 0,

		/// <summary>
		///     The user setting level. Defines in user personal cabinet.
		/// </summary>
		User = 1
	}
}