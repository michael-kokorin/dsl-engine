namespace Modules.Core.Services.UI.Handlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Security;
	using Common.Time;
	using Common.Transaction;
	using Infrastructure.Plugins;
	using Modules.Core.Services.UI.Commands;
	using Repository;
	using Repository.Context;
	using Repository.Repositories;

	using Authorities = Common.Security.Authorities;

	[UsedImplicitly]
	internal sealed class UpdateProjectPluginSettingsCommandHandler :
		ProjectCommandHandler<UpdateProjectPluginSettingsCommand>
	{
		private readonly IProjectPluginSettingsProvider _projectPluginSettingsProvider;

		private readonly IPluginRepository _pluginRepository;

		public UpdateProjectPluginSettingsCommandHandler(
			[NotNull] IUserAuthorityValidator userAuthorityValidator,
			[NotNull] IWriteRepository<Projects> repositoryProjects,
			[NotNull] ITimeService timeService,
			[NotNull] IUnitOfWork unitOfWork,
			[NotNull] IUserPrincipal userPrincipal,
			[NotNull] IProjectPluginSettingsProvider projectPluginSettingsProvider,
			[NotNull] IPluginRepository pluginRepository)
			: base(
				userAuthorityValidator,
				repositoryProjects,
				timeService,
				unitOfWork,
				userPrincipal)
		{
			if (projectPluginSettingsProvider == null) throw new ArgumentNullException(nameof(projectPluginSettingsProvider));
			if (pluginRepository == null) throw new ArgumentNullException(nameof(pluginRepository));

			_projectPluginSettingsProvider = projectPluginSettingsProvider;
			_pluginRepository = pluginRepository;
		}

		protected override string RequestedAuthorityName => Authorities.UI.Project.Settings.EditVersionControl;

		protected override long? GetProjectIdForCommand(UpdateProjectPluginSettingsCommand command) => command.ProjectId;

		protected override void UpdateProject(Projects project, UpdateProjectPluginSettingsCommand command)
		{
			var plugin = _pluginRepository.GetById(command.PluginId);

			switch (plugin.Type)
			{
				case (int) PluginType.VersionControl:
					project.VcsPluginId = command.PluginId;
					break;
				case (int) PluginType.IssueTracker:
					project.ItPluginId = command.PluginId;
					break;
				default:
					throw new Exception("Incorrect plugin type");
			}

			_projectPluginSettingsProvider.SetValues(command.ProjectId, command.Settings);
		}
	}
}