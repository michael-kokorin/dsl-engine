using Common.Container;
using Common.Extensions;

namespace Infrastructure.Plugins.Extensions
{
    using JetBrains.Annotations;

    using Microsoft.Practices.Unity;

    using Infrastructure.Plugins.Common.Contracts;
    using Infrastructure.Plugins.Contracts;

    internal static class ContainerExtension
    {
        [UsedImplicitly]
        public static IUnityContainer RegisterPlugin<T>(this IUnityContainer container, T plugin, ReuseScope reuseScope)
            where T : IPlugin => container.RegisterType<ICorePlugin>(plugin.GetType(), reuseScope);

        [UsedImplicitly]
        public static IUnityContainer RegisterPlugin<T>(this IUnityContainer container, T plugin, string name, ReuseScope reuseScope)
            where T : IPlugin => container.RegisterType<ICorePlugin>(plugin.GetType(), name, reuseScope);

        [UsedImplicitly]
        public static IUnityContainer RegisterCommonPlugin<T>(this IUnityContainer container, T plugin, ReuseScope reuseScope)
            where T : IPlugin => container.RegisterType<IPlugin>(plugin.GetType(), reuseScope);

        [UsedImplicitly]
        public static IUnityContainer RegisterCommonPlugin<T>(this IUnityContainer container, T plugin, string name, ReuseScope reuseScope)
            where T : IPlugin => container.RegisterType<IPlugin>(plugin.GetType(), name, reuseScope);
    }
}