namespace Infrastructure.Telemetry
{
	using System;

	using JetBrains.Annotations;
	using Microsoft.Practices.Unity;

	using Repository;

	[UsedImplicitly]
	internal sealed class TelemetryRepositoryResolver : ITelemetryRepositoryResolver
	{
		private readonly IUnityContainer _unityContainer;

		public TelemetryRepositoryResolver([NotNull] IUnityContainer unityContainer)
		{
			if (unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));

			_unityContainer = unityContainer;
		}

		public IWriteRepository<T> Resolve<T>() where T : class, ITelemetry =>
			_unityContainer.Resolve<IWriteRepository<T>>();
	}
}