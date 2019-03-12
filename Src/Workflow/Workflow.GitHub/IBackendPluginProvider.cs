namespace Workflow.GitHub
{
	using Infrastructure.Plugins.Contracts;

	/// <summary>
	///   Provides methods to get plugin instance.
	/// </summary>
	public interface IBackendPluginProvider
	{
		/// <summary>
		///   Gets the plugin.
		/// </summary>
		/// <typeparam name="TPlugin">The type of the plugin.</typeparam>
		/// <param name="pluginId">The plugin identifier.</param>
		/// <param name="userId">The user identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>The plugin instance.</returns>
		TPlugin GetPlugin<TPlugin>(long pluginId, long userId, long projectId)
			where TPlugin: class, ICorePlugin;
	}
}