namespace Modules.Core.Environment
{
	using System.Linq;
	using System.Security.Principal;

	using JetBrains.Annotations;

	using Common.Command;
	using Common.Data;
	using Infrastructure.AD;
	using Infrastructure.Plugins.Common;
	using Modules.Core.Services.UI.Commands;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class EnvironmentProvider : IEnvironmentProvider
	{
		private readonly ICommandDispatcher _commandHandlerProvider;

		private readonly IDataSourceAccessInitializer _dataSourceAccessInitializer;

		private readonly IDataSourceInitializer _dataSourceInitializer;

		private readonly IPluginInitializer _pluginInitializer;

		private readonly IProjectRepository _projectRepository;

		private readonly IRoleProvider _roleProvider;

		public EnvironmentProvider(
			IProjectRepository projectRepository,
			ICommandDispatcher commandHandlerProvider,
			IDataSourceInitializer dataSourceInitializer,
			IPluginInitializer pluginInitializer,
			IRoleProvider roleProvider,
			IDataSourceAccessInitializer dataSourceAccessInitializer)
		{
			_projectRepository = projectRepository;
			_commandHandlerProvider = commandHandlerProvider;
			_dataSourceInitializer = dataSourceInitializer;
			_pluginInitializer = pluginInitializer;
			_roleProvider = roleProvider;
			_dataSourceAccessInitializer = dataSourceAccessInitializer;
		}

		/// <summary>
		///   Prepares environment.
		/// </summary>
		public void Prepare()
		{
			_pluginInitializer.InitializePlugins();

			CreateAdminRole();

			CreateTestProject();

			_dataSourceInitializer.Initialize();

			_dataSourceAccessInitializer.Initialize();
		}

		private void CreateTestProject()
		{
			const string projectAlias = "sdltest";

			if (!_projectRepository.Get(projectAlias).Any())
			{
				_commandHandlerProvider.Handle(new CreateProjectCommand
				{
					Alias = projectAlias,
					DefaultBranchName = "master",
					Description =
						@"<h2> Default project</h2>
<p> Default project settings for next user roles:</p>
<ul>
<li> Developer </li>
<li> Security officer </li>
</ul>",
					Name = "Positive SDL project"
				});
			}
		}

		private void CreateAdminRole()
		{
			var adminRole = _roleProvider.Get(null, "sdl_admin");

			var isEmpty = adminRole.Sid == null;

			var adminRoleGroup = _roleProvider.TryCreateGroup(adminRole);

			adminRole.Sid = adminRoleGroup.Sid;

			_projectRepository.Save();

			if (isEmpty)
				_roleProvider.AddUser(adminRole.Id, WindowsIdentity.GetCurrent().User?.Value);
		}
	}
}