namespace Modules.Core.Services.UI.Queries
{
	using Common.Query;

	internal sealed class GetTaskResultsQuery : IDataQuery
	{
		public readonly long TaskId;

		public readonly long UserId;

		public GetTaskResultsQuery(long taskId, long userId)
		{
			TaskId = taskId;
			UserId = userId;
		}
	}
}