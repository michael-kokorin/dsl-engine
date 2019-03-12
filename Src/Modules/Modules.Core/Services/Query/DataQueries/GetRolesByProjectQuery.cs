namespace Modules.Core.Services.Query.DataQueries
{
	using Common.Query;

	internal sealed class GetRolesByProjectQuery : IDataQuery
	{
		public readonly long ProjectId;

		public GetRolesByProjectQuery(long projectId)
		{
			ProjectId = projectId;
		}
	}
}