namespace Modules.Core.Services.UI.QueryHandlers
{
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Query;
	using Infrastructure.Plugins;
	using Modules.Core.Contracts.UI.Dto.UserSettings;
	using Modules.Core.Services.UI.Queries;
	using Modules.Core.Services.UI.Renderers;

	[UsedImplicitly]
	internal sealed class GetPluginsByTypeQueryHandler : IDataQueryHandler<GetPluginsByTypeQuery, PluginDto[]>
	{
		private readonly IPluginProvider _pluginProvider;

		public GetPluginsByTypeQueryHandler(IPluginProvider pluginProvider)
		{
			_pluginProvider = pluginProvider;
		}

		/// <summary>
		///   Executes the specified data query.
		/// </summary>
		/// <param name="dataQuery">The data query.</param>
		/// <returns>The result of execution.</returns>
		public PluginDto[] Execute(GetPluginsByTypeQuery dataQuery) =>
			_pluginProvider.Get(_ => _.Type == (int)dataQuery.PluginType)
				.Select(new PluginRenderer().GetSpec())
				.ToArray();
	}
}