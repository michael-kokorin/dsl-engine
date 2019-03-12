namespace Infrastructure.Extensions
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Infrastructure.Telemetry;

	internal static class ContainerExtension
	{
		public static IUnityContainer RegisterEntityTelemetryCreator<TEntity, TCreator>(this IUnityContainer container,
			ReuseScope reuseScope)
			where TEntity : class
			where TCreator : IEntityTelemetryCreator<TEntity> =>
				container.RegisterType<IEntityTelemetryCreator<TEntity>, TCreator>(reuseScope);
	}
}