using Common.Enums;

namespace Infrastructure.Plugins
{
	using System.Collections.Generic;

	using Infrastructure.Plugins.Common.Contracts;

	public interface IPluginSettingProvider
	{
		/// <summary>
		///     Initializes the specified plugin settings in database
		/// </summary>
		/// <param name="plugin">The plugin instance.</param>
		/// <param name="pluginType">Plugin type</param>
		void Initialize(IPlugin plugin, PluginType pluginType);

		/// <summary>
		///     Gets the setting values for plugin.
		/// </summary>
		/// <param name="pluginId">The plugin identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="userId">The user identifier.</param>
		/// <returns>Setting values dictionary for specified plugin</returns>
		IDictionary<string, string> GetSettingsForPlugin(long pluginId, long projectId, long? userId = null);
	}
}