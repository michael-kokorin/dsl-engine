namespace Modules.Core.Services.UI.Queries
{
	using Common.Query;

	internal sealed class GetPluginsByProjectQuery : IDataQuery
	{
		public readonly long ProjectId;

		public GetPluginsByProjectQuery(long projectId)
		{
			ProjectId = projectId;
		}
	}
}