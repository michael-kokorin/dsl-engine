namespace Infrastructure.Telemetry
{
	using Repository;

	public interface ITelemetryRouter
	{
		void Send<T>(T telemetry) where T : class, ITelemetry;
	}
}