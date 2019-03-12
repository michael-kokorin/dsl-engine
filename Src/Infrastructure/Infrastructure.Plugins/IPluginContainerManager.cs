namespace Infrastructure.Plugins
{
	using JetBrains.Annotations;

	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;

	/// <summary>
	///     Manager for plugin containers
	/// </summary>
	internal interface IPluginContainerManager
	{
		/// <summary>
		///     Registers the specified plugin.
		/// </summary>
		/// <param name="plugin">The plugin.</param>
		/// <param name="pluginTypeName">The plugin type name</param>
		void Register([NotNull] IPlugin plugin, [NotNull] string pluginTypeName);

		/// <summary>
		///     Resolves the plugin of specified type name.
		/// </summary>
		/// <param name="pluginTypeName">Name of the plugin type name.</param>
		/// <returns>Plugin instance with specified type name.</returns>
		ICorePlugin Resolve([NotNull] string pluginTypeName);
	}
}