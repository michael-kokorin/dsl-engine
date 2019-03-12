namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage tables.
	/// </summary>
	public interface ITableRepository: ILocalizedRepository<Tables>
	{
		/// <summary>
		///   Gets tables by the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>Tables.</returns>
		IQueryable<Tables> Get(string name);

		/// <summary>
		///   Gets all available tables.
		/// </summary>
		/// <returns>Tables.</returns>
		IQueryable<Tables> GetAvailable();

		/// <summary>
		///   Gets available tables with the specified identifier.
		/// </summary>
		/// <param name="tableId">The table identifier.</param>
		/// <returns>Tables.</returns>
		IQueryable<Tables> GetAvailable(long tableId);
	}
}