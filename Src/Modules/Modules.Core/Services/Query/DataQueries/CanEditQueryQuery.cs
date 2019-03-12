namespace Modules.Core.Services.Query.DataQueries
{
	using Common.Query;

	internal sealed class CanEditQueryQuery : IDataQuery
	{
		public readonly long QueryId;

		public CanEditQueryQuery(long queryId)
		{
			QueryId = queryId;
		}
	}
}