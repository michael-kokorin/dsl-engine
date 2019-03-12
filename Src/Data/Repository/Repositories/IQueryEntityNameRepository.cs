namespace Repository.Repositories
{
	using Repository.Context;

	/// <summary>
	///   Provides methods to manage query entity names.
	/// </summary>
	public interface IQueryEntityNameRepository: IWriteRepository<QueryEntityNames>
	{
		/// <summary>
		///   Gets query entity name by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Query entity name.</returns>
		QueryEntityNames GetByKey(string key);
	}
}