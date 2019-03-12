namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage queue.
	/// </summary>
	public interface IQueueRepository: IWriteRepository<Queue>
	{
		/// <summary>
		///   Gets next item by the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>Next item in queue.</returns>
		IQueryable<Queue> GetNextByType(string type);
	}
}