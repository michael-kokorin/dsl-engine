namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provide methods to access authorities.
	/// </summary>
	public interface IAuthorityRepository: IWriteRepository<Authorities>
	{
		/// <summary>
		///   Gets authorities by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Authorities.</returns>
		IQueryable<Authorities> GetByKey(string key);

		/// <summary>
		///   Gets authorities by project and role.
		/// </summary>
		/// <param name="roleId">The role identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Authorities.</returns>
		// ReSharper disable once ReturnTypeCanBeEnumerable.Global
		// queryable here is required
		IQueryable<Authorities> GetByProjectAndRole(long roleId, long? projectId = null);
	}
}