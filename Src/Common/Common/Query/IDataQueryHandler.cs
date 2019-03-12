namespace Common.Query
{
	/// <summary>
	///   Provides methods to handle query.
	/// </summary>
	/// <typeparam name="TQuery">The type of the query.</typeparam>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	public interface IDataQueryHandler<in TQuery, out TResult>
		where TQuery: IDataQuery
	{
		/// <summary>
		///   Executes the specified data query.
		/// </summary>
		/// <param name="dataQuery">The data query.</param>
		/// <returns>The result of execution.</returns>
		TResult Execute(TQuery dataQuery);
	}
}