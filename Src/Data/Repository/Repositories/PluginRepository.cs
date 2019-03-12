namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Repository.Context;

	[UsedImplicitly]
	internal sealed class PluginRepository : Repository<Plugins>, IPluginRepository
	{
		public PluginRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets plugin by full name.
		/// </summary>
		/// <param name="typeFullName">Full name of the type.</param>
		/// <param name="assemblyName">Name of the assembly.</param>
		/// <param name="pluginType">Type of the plugin.</param>
		/// <returns>
		///   Plugin.
		/// </returns>
		public Plugins GetByType(string typeFullName, string assemblyName, PluginType pluginType) =>
			Query().SingleOrDefault(_ =>
				(_.AssemblyName == assemblyName) &&
				(_.TypeFullName == typeFullName) &&
				(_.Type == (int) pluginType));
	}
}