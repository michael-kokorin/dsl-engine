namespace Common.Query
{
	/// <summary>
	///   Provides methods to find appropriate query handler.
	/// </summary>
	internal interface IDataQueryHandlerProvider
	{
		/// <summary>
		///   Resolves the query handler..
		/// </summary>
		/// <typeparam name="TQuery">The type of the query.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <returns>The query handler.</returns>
		IDataQueryHandler<TQuery, TResult> Resolve<TQuery, TResult>() where TQuery: class, IDataQuery;
	}
}