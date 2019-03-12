namespace Repository.Repositories
{
	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class VcsPluginTelemetryRepository: Repository<VcsPluginTelemetry>, IVcsPluginTelemetryRepository
	{
		public VcsPluginTelemetryRepository([NotNull] IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}
	}
}