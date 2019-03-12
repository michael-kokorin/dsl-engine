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

	[UsedImplicitly]
	internal sealed class GetProjectTasksQueryHandler : IDataQueryHandler<GetProjectTasksQuery, TableDto>
	{
		private readonly IQueryExecutor _queryExecutor;

		private readonly IUserAuthorityValidator _userAuthorityValidator;

		private readonly IUserProvider _userProvider;

		private readonly IUserRoleProvider _userRoleProvider;

		public GetProjectTasksQueryHandler([NotNull] IQueryExecutor queryExecutor,
			[NotNull] IUserAuthorityValidator userAuthorityValidator,
			[NotNull] IUserProvider userProvider,
			[NotNull] IUserRoleProvider userRoleProvider)
		{
			if (queryExecutor == null) throw new ArgumentNullException(nameof(queryExecutor));
			if (userAuthorityValidator == null) throw new ArgumentNullException(nameof(userAuthorityValidator));
			if (userProvider == null) throw new ArgumentNullException(nameof(userProvider));
			if (userRoleProvider == null) throw new ArgumentNullException(nameof(userRoleProvider));

			_queryExecutor = queryExecutor;
			_userAuthorityValidator = userAuthorityValidator;
			_userProvider = userProvider;
			_userRoleProvider = userRoleProvider;
		}

		public TableDto Execute(GetProjectTasksQuery dataQuery)
		{
			if (dataQuery == null)
				throw new ArgumentNullException(nameof(dataQuery));

			if (!_userAuthorityValidator.HasUserAuthorities(
				dataQuery.UserId,
				new[]
				{
					Authorities.UI.Project.Tasks.View
				},
				dataQuery.ProjectId))
			{
				throw new UnauthorizedAccessException();
			}

			var user = _userProvider.Get(dataQuery.UserId);

			var role = _userRoleProvider.GetUserPreferedRoleForProject(user, dataQuery.ProjectId);

			if (role == null)
				throw new UnauthorizedAccessException();

			var request = _queryExecutor.Execute(
				role.TasksQueryId,
				dataQuery.UserId,
				new KeyValuePair<string, string>(
					Variables.ProjectId,
					dataQuery.ProjectId.ToString()));

			var result = new TableRenderer().Render(request);

			return result;
		}
	}
}