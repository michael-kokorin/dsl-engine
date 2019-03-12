namespace Modules.Core.Services.UI.QueryHandlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Query;
	using Common.Security;
	using Infrastructure.AD;
	using Modules.Core.Contracts.UI.Dto;
	using Modules.Core.Services.UI.Queries;
	using Repository.Context;

	using Authorities = Common.Security.Authorities;

	[UsedImplicitly]
	internal sealed class GetUsersByProjectQueryHandler : IDataQueryHandler<GetUsersByProjectQuery, UserDto[]>
	{
		private readonly IRoleProvider _roleProvider;

		private readonly IUserAuthorityValidator _userAuthorityValidator;

		private readonly IUserPrincipal _userPrincipal;

		private readonly IUserRoleProvider _userRoleProvider;

		public GetUsersByProjectQueryHandler([NotNull] IRoleProvider roleProvider,
			[NotNull] IUserAuthorityValidator userAuthorityValidator,
			[NotNull] IUserPrincipal userPrincipal,
			[NotNull] IUserRoleProvider userRoleProvider)
		{
			if (roleProvider == null) throw new ArgumentNullException(nameof(roleProvider));
			if (userAuthorityValidator == null) throw new ArgumentNullException(nameof(userAuthorityValidator));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));
			if (userRoleProvider == null) throw new ArgumentNullException(nameof(userRoleProvider));

			_roleProvider = roleProvider;
			_userAuthorityValidator = userAuthorityValidator;
			_userPrincipal = userPrincipal;
			_userRoleProvider = userRoleProvider;
		}

		public UserDto[] Execute(GetUsersByProjectQuery dataQuery)
		{
			if (dataQuery == null)
				throw new ArgumentNullException(nameof(dataQuery));

			var canViewProjectUsers = _userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[] {Authorities.UI.Project.ProjectsList.View},
				dataQuery.ProjectId);

			if (!canViewProjectUsers)
				throw new UnauthorizedAccessException();

			var roles = _roleProvider.Get(dataQuery.ProjectId);

			var users = new List<Users>();

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var role in roles)
			{
				var roleUsers = _userRoleProvider.GetUsersByRole(role);

				users.AddRange(roleUsers);
			}

			var userDtos = users
				.Select(_ => new UserDto
				{
					CurrentCulture = null,
					DisplayName = _.DisplayName,
					Email = _.Email,
					Id = _.Id,
					Login = _.Login,
					Sid = _.Sid
				})
				.ToArray();

			return userDtos;
		}
	}
}