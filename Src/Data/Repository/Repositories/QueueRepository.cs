namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class QueueRepository: Repository<Queue>, IQueueRepository
	{
		public QueueRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets next item by the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		///   Next item in queue.
		/// </returns>
		public IQueryable<Queue> GetNextByType(string type) => Query()
			.Where(_ => (_.Type == type) && (_.IsProcessed == false))
			.OrderBy(_ => _.Created);
	}
}