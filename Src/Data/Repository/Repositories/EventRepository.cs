namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class EventRepository: Repository<Events>, IEventRepository
	{
		public EventRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets event by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		///   Event.
		/// </returns>
		public IQueryable<Events> GetByKey(string key) => Query().Where(_ => _.Key == key);
	}
}