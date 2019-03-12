namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class RoleAuthorityRepository: Repository<RoleAuthorities>, IRoleAuthorityRepository
	{
		public RoleAuthorityRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets authorities by role and authority.
		/// </summary>
		/// <param name="roleId">The role identifier.</param>
		/// <param name="authorityId">The authority identifier.</param>
		/// <returns>
		///   Authorities.
		/// </returns>
		public IQueryable<RoleAuthorities> GetByRoleAndAuthority(long roleId, long authorityId)
			=> Query().Where(_ => (_.AuthorityId == authorityId) && (_.RoleId == roleId));
	}
}