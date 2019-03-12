namespace Infrastructure.Plugins
{
	using Infrastructure.Plugins.Contracts;

	/// <summary>
	///     Activates plugin instance
	/// </summary>
	internal interface IPluginActivator
	{
		/// <summary>
		///     Activates the specified plugin.
		/// </summary>
		/// <param name="pluginId">The plugin identifier.</param>
		/// <returns>Plugin instance</returns>
		ICorePlugin Activate(long pluginId);
	}
}