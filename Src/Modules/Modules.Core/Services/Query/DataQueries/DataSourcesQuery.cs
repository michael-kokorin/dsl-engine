namespace Modules.Core.Services.Query.DataQueries
{
	using Common.Query;

	internal sealed class DataSourcesQuery : IDataQuery
	{
		public readonly long? ProjectId;

		public DataSourcesQuery(long? projectId)
		{
			ProjectId = projectId;
		}
	}
}