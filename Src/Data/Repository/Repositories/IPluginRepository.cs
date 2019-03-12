namespace Repository.Repositories
{
	using Common.Enums;
	using Repository.Context;

	/// <summary>
	///   Provide methods to access plugins.
	/// </summary>
	public interface IPluginRepository: IWriteRepository<Plugins>
	{
		/// <summary>
		///   Gets plugin by full name.
		/// </summary>
		/// <param name="typeFullName">Full name of the type.</param>
		/// <param name="assemblyName">Name of the assembly.</param>
		/// <param name="pluginType">Type of the plugin.</param>
		/// <returns>Plugin.</returns>
		Plugins GetByType(string typeFullName, string assemblyName, PluginType pluginType);
	}
}