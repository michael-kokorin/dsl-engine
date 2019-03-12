namespace Workflow.GitHub
{
	using System;

	using JetBrains.Annotations;

	using Common.Security;
	using Infrastructure.Plugins;
	using Infrastructure.Plugins.Contracts;

	[UsedImplicitly]
	internal sealed class BackendPluginProvider : IBackendPluginProvider
	{
		private readonly IPluginFactory _pluginFactory;

		private readonly IUserPrincipal _userPrincipal;

		public BackendPluginProvider([NotNull] IPluginFactory pluginFactory, [NotNull] IUserPrincipal userPrincipal)
		{
			if (pluginFactory == null) throw new ArgumentNullException(nameof(pluginFactory));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_pluginFactory = pluginFactory;
			_userPrincipal = userPrincipal;
		}

		/// <summary>
		///   Gets the plugin.
		/// </summary>
		/// <typeparam name="TPlugin">The type of the plugin.</typeparam>
		/// <param name="pluginId">The plugin identifier.</param>
		/// <param name="userId">The user identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>The plugin instance.</returns>
		public TPlugin GetPlugin<TPlugin>(long pluginId, long userId, long projectId)
			where TPlugin : class, ICorePlugin
			=> (userId == _userPrincipal.Info.Id
				? _pluginFactory.Prepare(pluginId, projectId)
				: _pluginFactory.Prepare(pluginId, projectId, userId)) as TPlugin;
	}
}