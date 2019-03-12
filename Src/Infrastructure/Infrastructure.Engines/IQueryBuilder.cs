namespace Infrastructure.Engines
{
	using System.Collections;
	using System.Collections.Generic;

	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Query.Result;

	/// <summary>
	///     Provides methods to execute data queries.
	/// </summary>
	public interface IQueryBuilder
	{
		/// <summary>
		///     Executes the specified query with the specified parameters and returns the result as table.
		/// </summary>
		/// <param name="query">The data query.</param>
		/// <param name="parameters">The query parameters.</param>
		/// <returns>The result of query.</returns>
		IEnumerable<QueryResultItem> ExecuteTable(string query, params KeyValuePair<string, string>[] parameters);

		/// <summary>
		/// Executes the specified query expr.
		/// </summary>
		/// <param name="query">The query expr.</param>
		/// <returns>Thre resula of query as array of objects</returns>
		IEnumerable<QueryResultItem> ExecuteTable(DslDataQuery query);

		/// <summary>
		///     Executes the specified query with the specified parameters and returns the result as collection.
		/// </summary>
		/// <param name="query">The data query.</param>
		/// <param name="parameters">The query parameters.</param>
		/// <returns>The result of query.</returns>
		IEnumerable ExecuteEnumerable(string query, params KeyValuePair<string, string>[] parameters);

		/// <summary>
		///     Executes the specified query with the specified parameters and returns the result.
		/// </summary>
		/// <param name="query">The data query.</param>
		/// <param name="parameters">The query parameters.</param>
		/// <returns>The result of query.</returns>
		object Execute(string query, params KeyValuePair<string, string>[] parameters);
	}
}