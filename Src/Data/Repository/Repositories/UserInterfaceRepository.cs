namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class UserInterfaceRepository: Repository<UserInterfaces>, IUserInterfaceRepository
	{
		public UserInterfaceRepository([NotNull] IDbContextProvider dbContextProvider)
			: base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets UIs by the specified host.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <returns>
		///   UIs
		/// </returns>
		public IQueryable<UserInterfaces> Get(string host) =>
			Query().Where(_ => _.Host == host);
	}
}