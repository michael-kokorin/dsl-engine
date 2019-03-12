namespace Repository.Repositories
{
	using System.Collections.Generic;
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage queries.
	/// </summary>
	public interface IQueryRepository: IWriteRepository<Queries>
	{
		/// <summary>
		///   Gets queries by specified query ids.
		/// </summary>
		/// <param name="queryIds">The query ids.</param>
		/// <returns>Queries.</returns>
		IQueryable<Queries> Get(IEnumerable<long> queryIds);

		/// <summary>
		///   Gets queries by the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Queries.</returns>
		IQueryable<Queries> Get(string name, long? projectId = null);

		/// <summary>
		///   Gets queries by the specified user identifier.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="name">The name.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Queries.</returns>
		Queries Get(long userId, string name, long? projectId = null);

		/// <summary>
		///   Gets queries by the specified user identifier.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Queries.</returns>
		IQueryable<Queries> Get(long userId, long? projectId);

		/// <summary>
		///   Gets queries by the specified project identifier.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Queries.</returns>
		IQueryable<Queries> Get(long projectId);
	}
}