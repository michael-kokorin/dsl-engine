namespace Infrastructure.Telemetry
{
	using JetBrains.Annotations;

	public interface IEntityTelemetryCreator<in T>
		where T :  class
	{
		void Save(
			[NotNull] string operationName,
			long? operationDuration,
			TelemetryOperationStatus telemetryOperationStatus,
			int? hResult,
			T sourceEntity);
	}
}