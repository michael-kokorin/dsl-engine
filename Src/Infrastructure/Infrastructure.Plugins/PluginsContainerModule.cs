using Common.Container;
using Common.Extensions;

namespace Infrastructure.Plugins
{
	using Microsoft.Practices.Unity;

	using Infrastructure.Plugins.Common;

	public sealed class PluginsContainerModule : IContainerModule
	{
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) =>
			container
				.RegisterType<IProjectPluginSettingsProvider, ProjectPluginSettingsProvider>(reuseScope)
				.RegisterType<IUserPluginSettingsProvider, UserPluginSettingsProvider>(reuseScope)
				.RegisterType<IPluginProvider, PluginProvider>(reuseScope)
				.RegisterType<IPluginSettingProvider, PluginSettingProvider>(reuseScope)
				.RegisterType<IPluginActivator, PluginActivator>(reuseScope)
				.RegisterType<IPluginFactory, PluginFactory>(reuseScope)
				.RegisterType<IPluginInitializer, PluginInitializer>(reuseScope)
				.RegisterType<IPluginContainerManager, PluginContainerManager>(reuseScope);
	}
}