namespace Modules.Core.Services.Query.DataQueries
{
	using Common.Query;

	public sealed class GetQueryResultsQuery : IDataQuery
	{
		public QueryParameterValue[] Parameters;

		public readonly long QueryId;

		public long? UserId;

		public GetQueryResultsQuery(long queryId, long? userId, QueryParameterValue[] parameters = null)
		{
			Parameters = parameters;

			QueryId = queryId;

			UserId = userId;
		}
	}
}