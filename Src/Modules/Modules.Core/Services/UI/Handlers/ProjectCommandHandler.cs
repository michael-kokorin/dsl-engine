namespace Modules.Core.Services.UI.Handlers
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Common.Command;
	using Common.Security;
	using Common.Time;
	using Common.Transaction;
	using Repository;
	using Repository.Context;

	internal abstract class ProjectCommandHandler<T> : CommandHandler<T>
		where T : class, ICommand
	{
		private readonly IWriteRepository<Projects> _repositoryProjects;

		private readonly ITimeService _timeService;

		private readonly IUserPrincipal _userPrincipal;

		protected ProjectCommandHandler(
			[NotNull] IUserAuthorityValidator userAuthorityValidator,
			[NotNull] IWriteRepository<Projects> repositoryProjects,
			[NotNull] ITimeService timeService,
			[NotNull] IUnitOfWork unitOfWork,
			[NotNull] IUserPrincipal userPrincipal)
			: base(userAuthorityValidator, unitOfWork, userPrincipal)
		{
			_repositoryProjects = repositoryProjects;
			_timeService = timeService;
			_userPrincipal = userPrincipal;
		}

		protected override void ProcessAuthorized(T command)
		{
			var projectId = GetProjectIdForCommand(command);

			if (projectId == null)
				return;

			var project = _repositoryProjects.GetById(projectId.Value);

			if (project == null)
				throw new KeyNotFoundException();

			UpdateProject(project, command);

			project.Modified = _timeService.GetUtc();
			project.ModifiedById = _userPrincipal.Info.Id;

			_repositoryProjects.Save();
		}

		protected abstract void UpdateProject(Projects project, T command);
	}
}