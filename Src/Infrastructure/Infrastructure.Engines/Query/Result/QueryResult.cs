namespace Infrastructure.Engines.Query.Result
{
	using System.Collections.Generic;

	public sealed class QueryResult
	{
		public IEnumerable<QueryException> Exceptions { get; set; }

		public IEnumerable<QueryResultColumn> Columns { get; set; }

		public IEnumerable<QueryResultItem> Items { get; set; }
	}
}