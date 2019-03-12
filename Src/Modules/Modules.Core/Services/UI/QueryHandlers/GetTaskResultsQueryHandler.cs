namespace Modules.Core.Services.UI.QueryHandlers
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Common.Query;
	using Common.Security;
	using Infrastructure;
	using Infrastructure.AD;
	using Infrastructure.Query;
	using Modules.Core.Contracts.UI.Dto.Data;
	using Modules.Core.Services.UI.Queries;
	using Modules.Core.Services.UI.Renderers;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class GetTaskResultsQueryHandler : IDataQueryHandler<GetTaskResultsQuery, TableDto>
	{
		private readonly IQueryExecutor _queryExecutor;

		private readonly ITaskRepository _taskRepository;

		private readonly IUserAuthorityValidator _userAuthorityValidator;

		private readonly IUserProvider _userProvider;

		private readonly IUserRoleProvider _userRoleProvider;

		public GetTaskResultsQueryHandler([NotNull] IQueryExecutor queryExecutor,
			[NotNull] ITaskRepository taskRepository,
			[NotNull] IUserAuthorityValidator userAuthorityValidator,
			[NotNull] IUserProvider userProvider,
			[NotNull] IUserRoleProvider userRoleProvider)
		{
			if (queryExecutor == null) throw new ArgumentNullException(nameof(queryExecutor));
			if (taskRepository == null) throw new ArgumentNullException(nameof(taskRepository));
			if (userAuthorityValidator == null) throw new ArgumentNullException(nameof(userAuthorityValidator));
			if (userProvider == null) throw new ArgumentNullException(nameof(userProvider));
			if (userRoleProvider == null) throw new ArgumentNullException(nameof(userRoleProvider));

			_queryExecutor = queryExecutor;
			_taskRepository = taskRepository;
			_userAuthorityValidator = userAuthorityValidator;
			_userProvider = userProvider;
			_userRoleProvider = userRoleProvider;
		}

		public TableDto Execute(GetTaskResultsQuery dataQuery)
		{
			if (dataQuery == null)
				throw new ArgumentNullException(nameof(dataQuery));

			var task = _taskRepository.GetById(dataQuery.TaskId);

			if (task == null)
				return null;

			if (!_userAuthorityValidator.HasUserAuthorities(
				dataQuery.UserId,
				new[]
				{
					Authorities.UI.Project.Tasks.ViewResults
				},
				task.ProjectId))
				throw new UnauthorizedAccessException();

			var user = _userProvider.Get(dataQuery.UserId);

			var role = _userRoleProvider.GetUserPreferedRoleForProject(user, task.ProjectId);

			if (role == null)
				throw new UnauthorizedAccessException();

			var query = _queryExecutor.Execute(
				role.TaskResultsQueryId,
				dataQuery.UserId,
				new KeyValuePair<string, string>(
					Variables.TaskId,
					dataQuery.TaskId.ToString()));

			return new TableRenderer().Render(query);
		}
	}
}