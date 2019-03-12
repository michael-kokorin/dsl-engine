using Common.Container;
using Common.Extensions;

namespace Infrastructure.Plugins.Common
{
	using Microsoft.Practices.Unity;

	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;

	public sealed class CommonPluginsContainerModule: IContainerModule
	{
		/// <summary>
		///     Registers binding to the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) =>
			container
				.RegisterType<IPluginDirectoryNameProvider, PluginDirectoryNameProvider>(reuseScope)
				.RegisterType<IPluginLoader<IPlugin>, PluginLoader<IPlugin>>(reuseScope)
				.RegisterType<IPluginLoader<ICorePlugin>, PluginLoader<ICorePlugin>>(reuseScope)
				.RegisterType<IPluginDirectoriesProvider, PluginDirectoriesProvider>(reuseScope);
	}
}