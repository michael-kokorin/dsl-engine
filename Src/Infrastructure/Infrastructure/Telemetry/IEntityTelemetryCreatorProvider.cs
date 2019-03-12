namespace Infrastructure.Telemetry
{
	internal interface IEntityTelemetryCreatorProvider
	{
		IEntityTelemetryCreator<T> Resolve<T>() where T : class ;
	}
}