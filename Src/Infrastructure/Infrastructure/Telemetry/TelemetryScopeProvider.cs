namespace Infrastructure.Telemetry
{
	using System;

	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class TelemetryScopeProvider : ITelemetryScopeProvider
	{
		private readonly IEntityTelemetryCreatorProvider _entityTelemetryCreatorProvider;

		public TelemetryScopeProvider([NotNull] IEntityTelemetryCreatorProvider entityTelemetryCreatorProvider)
		{
			if (entityTelemetryCreatorProvider == null) throw new ArgumentNullException(nameof(entityTelemetryCreatorProvider));

			_entityTelemetryCreatorProvider = entityTelemetryCreatorProvider;
		}

		public ITelemetryScope<T> Create<T>([NotNull] string operationName) where T : class
		{
			if (operationName == null) throw new ArgumentNullException(nameof(operationName));

			var entityTelemetryProvider = _entityTelemetryCreatorProvider.Resolve<T>();

			return new TelemetryScope<T>(entityTelemetryProvider, operationName);
		}
	}
}