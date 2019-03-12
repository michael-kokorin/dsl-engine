namespace Repository.Repositories
{
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class RoleRepository: Repository<Roles>, IRoleRepository
	{
		public RoleRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets the active roles.
		/// </summary>
		/// <returns>
		///   Roles.
		/// </returns>
		public IQueryable<Roles> GetActive() => Query().Where(_ => _.Sid != null);

		/// <summary>
		///   Gets roles by the specified project identifier.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="alias">The alias.</param>
		/// <returns>
		///   Roles.
		/// </returns>
		public IQueryable<Roles> Get(long? projectId, string alias)
			=> Query().Where(_ => (_.ProjectId == projectId) && (_.Alias == alias));

		/// <summary>
		///   Gets roles by project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		///   Roles.
		/// </returns>
		public IQueryable<Roles> GetByProject(long? projectId) =>
			GetActive().Where(_ => (_.ProjectId == null) || (_.ProjectId == projectId));

		/// <summary>
		///   Gets roles by the specified authorities.
		/// </summary>
		/// <param name="authorities">The authorities.</param>
		/// <returns>
		///   Roles.
		/// </returns>
		public IQueryable<Roles> Get(IEnumerable<string> authorities) =>
			GetActive().Where(_ => _.RoleAuthorities.Any(ra => authorities.Contains(ra.Authorities.Key)));

		/// <summary>
		///   Gets roles by authorities.
		/// </summary>
		/// <param name="authorities">The authorities.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		///   Roles.
		/// </returns>
		public IQueryable<Roles> GetByAuthorities(IEnumerable<string> authorities, long? projectId = null) =>
			GetActive().Where(
				r =>
				((r.ProjectId == projectId) || (r.ProjectId == null)) &&
				r.RoleAuthorities.Any(a => authorities.Contains(a.Authorities.Key)));
	}
}