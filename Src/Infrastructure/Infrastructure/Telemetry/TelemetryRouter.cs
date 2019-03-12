namespace Infrastructure.Telemetry
{
	using System;

	using JetBrains.Annotations;

	using Repository;

	[UsedImplicitly]
	internal sealed class TelemetryRouter : ITelemetryRouter
	{
		private readonly ITelemetryRepositoryResolver _telemetryRepositoryResolver;

		public TelemetryRouter([NotNull] ITelemetryRepositoryResolver telemetryRepositoryResolver)
		{
			if (telemetryRepositoryResolver == null) throw new ArgumentNullException(nameof(telemetryRepositoryResolver));

			_telemetryRepositoryResolver = telemetryRepositoryResolver;
		}

		public void Send<T>([NotNull] T telemetry) where T : class, ITelemetry
		{
			if (telemetry == null) throw new ArgumentNullException(nameof(telemetry));

			var repository = _telemetryRepositoryResolver.Resolve<T>();

			repository.Insert(telemetry);

			repository.Save();
		}
	}
}