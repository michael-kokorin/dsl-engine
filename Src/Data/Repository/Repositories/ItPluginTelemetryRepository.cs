namespace Repository.Repositories
{
	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class ItPluginTelemetryRepository: Repository<ItPluginTelemetry>, IItPluginTelemetryRepository
	{
		public ItPluginTelemetryRepository([NotNull] IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}
	}
}