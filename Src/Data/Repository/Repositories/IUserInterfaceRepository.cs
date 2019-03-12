namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage UIs.
	/// </summary>
	public interface IUserInterfaceRepository: IWriteRepository<UserInterfaces>
	{
		/// <summary>
		///   Gets UIs by the specified host.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <returns>UIs</returns>
		IQueryable<UserInterfaces> Get(string host);
	}
}