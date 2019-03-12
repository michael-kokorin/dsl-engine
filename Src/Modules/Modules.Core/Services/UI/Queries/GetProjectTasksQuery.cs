namespace Modules.Core.Services.UI.Queries
{
	using Common.Query;

	public sealed class GetProjectTasksQuery : IDataQuery
	{
		public readonly long ProjectId;

		public readonly long UserId;

		public GetProjectTasksQuery(long projectId, long userId)
		{
			ProjectId = projectId;

			UserId = userId;
		}
	}
}