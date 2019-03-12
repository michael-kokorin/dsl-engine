namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class AuthorityRepository: Repository<Authorities>, IAuthorityRepository
	{
		public AuthorityRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets authorities by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Authorities.</returns>
		public IQueryable<Authorities> GetByKey(string key) =>
			Query().Where(_ => _.Key == key);

		/// <summary>
		///   Gets authorities by project and role.
		/// </summary>
		/// <param name="roleId">The role identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Authorities.</returns>
		// ReSharper disable once ReturnTypeCanBeEnumerable.Global
		// queryable here is required
		public IQueryable<Authorities> GetByProjectAndRole(long roleId, long? projectId = null) =>
			Query().Where(
				_ =>
				_.RoleAuthorities.Any(
					r =>
					(r.RoleId == roleId) &&
					((r.Roles.ProjectId == projectId) ||
					(r.Roles.ProjectId == null))));
	}
}