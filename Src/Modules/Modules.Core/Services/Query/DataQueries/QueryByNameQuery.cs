namespace Modules.Core.Services.Query.DataQueries
{
	using Common.Query;

	internal sealed class QueryByNameQuery : IDataQuery
	{
		public readonly string Name;

		public readonly long? ProjectId;

		public QueryByNameQuery(string name, long? projectId)
		{
			Name = name;
			ProjectId = projectId;
		}
	}
}