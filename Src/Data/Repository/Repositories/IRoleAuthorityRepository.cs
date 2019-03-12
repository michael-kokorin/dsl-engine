namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage role authorities.
	/// </summary>
	public interface IRoleAuthorityRepository: IWriteRepository<RoleAuthorities>
	{
		/// <summary>
		///   Gets authorities by role and authority.
		/// </summary>
		/// <param name="roleId">The role identifier.</param>
		/// <param name="authorityId">The authority identifier.</param>
		/// <returns>Authorities.</returns>
		IQueryable<RoleAuthorities> GetByRoleAndAuthority(long roleId, long authorityId);
	}
}