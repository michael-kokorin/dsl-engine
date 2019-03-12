namespace Infrastructure.Query
{
	using System.Collections.Generic;

	using Infrastructure.Engines.Query.Result;

	public interface IQueryExecutor
	{
		QueryResult Execute(long queryId,
			long? userId = null,
			params KeyValuePair<string, string>[] parameters);

		QueryResult Execute(string query,
			long? userId = null,
			params KeyValuePair<string, string>[] parameters);
	}
}