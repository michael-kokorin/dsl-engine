namespace Repository.Repositories
{
	using System.Collections.Generic;
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage roles.
	/// </summary>
	public interface IRoleRepository: IWriteRepository<Roles>
	{
		/// <summary>
		///   Gets roles by the specified project identifier.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="alias">The alias.</param>
		/// <returns>Roles.</returns>
		IQueryable<Roles> Get(long? projectId, string alias);

		/// <summary>
		///   Gets roles by the specified authorities.
		/// </summary>
		/// <param name="authorities">The authorities.</param>
		/// <returns>Roles.</returns>
		IQueryable<Roles> Get(IEnumerable<string> authorities);

		/// <summary>
		///   Gets the active roles.
		/// </summary>
		/// <returns>Roles.</returns>
		IQueryable<Roles> GetActive();

		/// <summary>
		///   Gets roles by authorities.
		/// </summary>
		/// <param name="authorities">The authorities.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Roles.</returns>
		IQueryable<Roles> GetByAuthorities(IEnumerable<string> authorities, long? projectId = null);

		/// <summary>
		///   Gets roles by project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Roles.</returns>
		IQueryable<Roles> GetByProject(long? projectId);
	}
}