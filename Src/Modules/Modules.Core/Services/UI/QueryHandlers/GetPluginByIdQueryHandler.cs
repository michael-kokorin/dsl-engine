namespace Modules.Core.Services.UI.QueryHandlers
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Query;
	using Infrastructure.Plugins;
	using Modules.Core.Contracts.UI.Dto.UserSettings;
	using Modules.Core.Services.UI.Queries;
	using Modules.Core.Services.UI.Renderers;

	[UsedImplicitly]
	internal sealed class GetPluginByIdQueryHandler : IDataQueryHandler<GetPluginByIdQuery, PluginDto>
	{
		private readonly IPluginProvider _pluginProvider;

		public GetPluginByIdQueryHandler([NotNull] IPluginProvider pluginProvider)
		{
			if (pluginProvider == null) throw new ArgumentNullException(nameof(pluginProvider));

			_pluginProvider = pluginProvider;
		}

		/// <summary>
		///   Executes the specified data query.
		/// </summary>
		/// <param name="dataQuery">The data query.</param>
		/// <returns>The result of execution.</returns>
		public PluginDto Execute([NotNull] GetPluginByIdQuery dataQuery)
		{
			if (dataQuery == null) throw new ArgumentNullException(nameof(dataQuery));

			var plugin = _pluginProvider.Get(_ => _.Id == dataQuery.PluginId)
				.Select(new PluginRenderer().GetSpec())
				.SingleOrDefault();

			return plugin;
		}
	}
}