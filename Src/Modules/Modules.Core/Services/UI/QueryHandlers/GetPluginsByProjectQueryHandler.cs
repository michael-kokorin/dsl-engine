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
	internal sealed class GetPluginsByProjectQueryHandler : IDataQueryHandler<GetPluginsByProjectQuery, PluginDto[]>
	{
		private readonly IPluginProvider _pluginsProvider;

		public GetPluginsByProjectQueryHandler([NotNull] IPluginProvider pluginsProvider)
		{
			if (pluginsProvider == null) throw new ArgumentNullException(nameof(pluginsProvider));

			_pluginsProvider = pluginsProvider;
		}

		/// <summary>
		///   Executes the specified data query.
		/// </summary>
		/// <param name="dataQuery">The data query.</param>
		/// <returns>The result of execution.</returns>
		public PluginDto[] Execute([NotNull] GetPluginsByProjectQuery dataQuery)
		{
			if (dataQuery == null) throw new ArgumentNullException(nameof(dataQuery));

			return _pluginsProvider.Get(_ =>
				_.Projects.Any(p => p.Id == dataQuery.ProjectId && p.ItPluginId == _.Id) ||
				_.Projects1.Any(p => p.Id == dataQuery.ProjectId && p.VcsPluginId == _.Id))
				.Select(new PluginRenderer().GetSpec())
				.ToArray();
		}
	}
}