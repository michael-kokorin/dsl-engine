namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class ProjectRepository: Repository<Projects>, IProjectRepository
	{
		public ProjectRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets project by the specified alias.
		/// </summary>
		/// <param name="alias">The alias.</param>
		/// <returns>
		///   Projects.
		/// </returns>
		public IQueryable<Projects> Get(string alias) =>
			Query().Where(_ => _.Alias == alias);

		/// <summary>
		///   Get project which have VCS sync option enabled.
		/// </summary>
		/// <returns>
		///   Projects.
		/// </returns>
		public IQueryable<Projects> VcsSyncEnabled() =>
			Query().Where(_ => _.VcsSyncEnabled);
	}
}