namespace Infrastructure.Plugins
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	/// <summary>
	///     Provider for plugin settings in project
	/// </summary>
	public interface IProjectPluginSettingsProvider
	{
		/// <summary>
		///     Gets the settings for plugin in project
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="pluginid">The pluginid.</param>
		/// <returns>Plugin settings in project</returns>
		[NotNull]
		[ItemNotNull]
		IEnumerable<PluginSetting> GetSettings(long projectId, long pluginid);

		/// <summary>
		///     Sets the values of plugin settings in project.
		/// </summary>
		/// <param name="projectId">Target project identifier</param>
		/// <param name="values">The values.</param>
		void SetValues(long projectId, [NotNull] [ItemNotNull] IEnumerable<ProjectPluginSetting> values);
	}
}