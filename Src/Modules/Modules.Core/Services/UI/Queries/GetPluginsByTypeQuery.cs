namespace Modules.Core.Services.UI.Queries
{
	using Common.Enums;
	using Common.Extensions;
	using Common.Query;
	using Modules.Core.Contracts.UI.Dto.UserSettings;

	internal sealed class GetPluginsByTypeQuery : IDataQuery
	{
		public readonly PluginType PluginType;

		public GetPluginsByTypeQuery(PluginTypeDto pluginTypeDto)
		{
			PluginType = pluginTypeDto.GetEqualByValue<PluginType>();
		}
	}
}