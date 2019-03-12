namespace Infrastructure.Telemetry
{
	using Repository;

	internal interface ITelemetryRepositoryResolver
	{
		IWriteRepository<T> Resolve<T>() where T : class, ITelemetry;
	}
}