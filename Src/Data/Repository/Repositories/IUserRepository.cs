namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage users.
	/// </summary>
	public interface IUserRepository: IWriteRepository<Users>
	{
		/// <summary>
		///   Gets users by specified user identifier.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <returns>Users.</returns>
		IQueryable<Users> Get(long userId);

		/// <summary>
		///   Gets users by sid.
		/// </summary>
		/// <param name="sid">The sid.</param>
		/// <returns>Users.</returns>
		IQueryable<Users> GetBySid(string sid);
	}
}