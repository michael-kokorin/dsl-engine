namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class UserRepository: Repository<Users>, IUserRepository
	{
		public UserRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets users by specified user identifier.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <returns>
		///   Users.
		/// </returns>
		public IQueryable<Users> Get(long userId) =>
			Query().Where(_ => _.Id == userId);

		/// <summary>
		///   Gets users by sid.
		/// </summary>
		/// <param name="sid">The sid.</param>
		/// <returns>
		///   Users.
		/// </returns>
		public IQueryable<Users> GetBySid(string sid) =>
			Query().Where(_ => _.Sid == sid);
	}
}