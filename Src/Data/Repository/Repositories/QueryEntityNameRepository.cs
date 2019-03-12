namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class QueryEntityNameRepository: Repository<QueryEntityNames>, IQueryEntityNameRepository
	{
		public QueryEntityNameRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets query entity name by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		///   Query entity name.
		/// </returns>
		public QueryEntityNames GetByKey(string key) => Query().FirstOrDefault(x => x.Key == key);
	}
}