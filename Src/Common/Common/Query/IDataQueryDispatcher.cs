namespace Common.Query
{
	/// <summary>
	///   Provides methods to process query.
	/// </summary>
	public interface IDataQueryDispatcher
	{
		/// <summary>
		///   Processes the specified query.
		/// </summary>
		/// <typeparam name="TQuery">The type of the query.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="query">The query.</param>
		/// <returns>The result of processing.</returns>
		TResult Process<TQuery, TResult>(TQuery query) where TQuery: class, IDataQuery;
	}
}