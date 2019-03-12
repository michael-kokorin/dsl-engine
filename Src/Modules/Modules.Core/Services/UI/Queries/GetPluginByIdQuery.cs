namespace Modules.Core.Services.UI.Queries
{
	using Common.Query;

	internal sealed class GetPluginByIdQuery : IDataQuery
	{
		public readonly long PluginId;

		public GetPluginByIdQuery(long pluginId)
		{
			PluginId = pluginId;
		}
	}
}