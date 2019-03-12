namespace Modules.Core.Services.UI.Queries
{
	using Common.Query;

	internal sealed class GetUsersByProjectQuery : IDataQuery
	{
		public readonly long? ProjectId;

		public GetUsersByProjectQuery(long? projectId)
		{
			ProjectId = projectId;
		}
	}
}