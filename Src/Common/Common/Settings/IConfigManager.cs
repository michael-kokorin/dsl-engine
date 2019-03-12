namespace Common.Settings
{
	/// <summary>
	///   Provides methods to work with application configuration manager.
	/// </summary>
	public interface IConfigManager
	{
		/// <summary>
		///   Gets value of the specified setting.
		/// </summary>
		/// <param name="settingName">Name of the setting.</param>
		/// <returns>The value of the specified setting.</returns>
		string Get(string settingName);

		/// <summary>
		///   Gets the connection string.
		/// </summary>
		/// <returns>The connection string.</returns>
		string GetConnectionString();

		/// <summary>
		///   Sets the specified setting.
		/// </summary>
		/// <param name="settingName">Name of the setting.</param>
		/// <param name="value">The value.</param>
		void Set(string settingName, string value);
	}
}