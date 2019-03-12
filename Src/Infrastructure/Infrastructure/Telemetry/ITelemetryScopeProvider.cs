namespace Infrastructure.Telemetry
{
	public interface ITelemetryScopeProvider
	{
		ITelemetryScope<T> Create<T>(string operationName)
			where T : class;
	}
}