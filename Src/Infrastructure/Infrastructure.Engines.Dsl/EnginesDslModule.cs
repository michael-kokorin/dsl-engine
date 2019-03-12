namespace Infrastructure.Engines.Dsl
{
    using Microsoft.Practices.Unity;

    using Common.Container;
    using Common.Extensions;

    public sealed class EnginesDslModule : IContainerModule
    {
        public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) =>
            container.RegisterType<IDslParser, DslParser>(reuseScope);
    }
}