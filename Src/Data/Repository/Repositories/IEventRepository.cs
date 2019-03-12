namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage events.
	/// </summary>
	public interface IEventRepository: IWriteRepository<Events>
	{
		/// <summary>
		///   Gets event by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Event.</returns>
		IQueryable<Events> GetByKey(string key);
	}
}