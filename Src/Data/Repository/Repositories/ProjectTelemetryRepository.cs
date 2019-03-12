namespace Repository.Repositories
{
	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class ProjectTelemetryRepository: Repository<ProjectTelemetry>, IProjectTelemetryRepository
	{
		public ProjectTelemetryRepository([NotNull] IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}
	}
}