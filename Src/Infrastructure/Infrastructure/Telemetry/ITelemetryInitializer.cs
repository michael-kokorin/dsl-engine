namespace Infrastructure.Telemetry
{
	using Repository;

	public interface ITelemetryInitializer
	{
		void Initialize<T>(T telemetry) where T : class, ITelemetry;
	}
}