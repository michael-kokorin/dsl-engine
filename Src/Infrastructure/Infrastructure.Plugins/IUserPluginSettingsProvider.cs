namespace Infrastructure.Plugins
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	/// <summary>
	///     Provides plugin settings management for user
	/// </summary>
	public interface IUserPluginSettingsProvider
	{
		/// <summary>
		///     Gets the settings for user in project
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="pluginId">The plugin identifier.</param>
		/// <returns>List of user settings in project</returns>
		[NotNull]
		[ItemNotNull]
		IEnumerable<PluginSetting> GetSettings(long userId, long projectId, long pluginId);

		/// <summary>
		///     Sets the plugin setting values.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="pluginSettingValues">The plugin setting values.</param>
		void SetValues(long userId, [NotNull] [ItemNotNull] IEnumerable<ProjectPluginSetting> pluginSettingValues);
	}
}