namespace Infrastructure.Plugins
{
	using Infrastructure.Plugins.Contracts;

	/// <summary>
	///     Prepares plugin for usage
	/// </summary>
	public interface IPluginFactory
	{
		/// <summary>
		///     Prepares the specified plugin for usage with specified project and user
		/// </summary>
		/// <param name="pluginId">The plugin identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="userId">The user identifier.</param>
		/// <returns>Initialized plugin for specified project and user</returns>
		ICorePlugin Prepare(long pluginId, long projectId, long? userId = null);
	}
}