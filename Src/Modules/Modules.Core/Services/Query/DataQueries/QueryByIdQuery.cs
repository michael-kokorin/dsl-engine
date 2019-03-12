namespace Modules.Core.Services.Query.DataQueries
{
	using Common.Query;

	internal sealed class QueryByIdQuery : IDataQuery
	{
		public readonly long QueryId;

		public QueryByIdQuery(long queryId)
		{
			QueryId = queryId;
		}
	}
}