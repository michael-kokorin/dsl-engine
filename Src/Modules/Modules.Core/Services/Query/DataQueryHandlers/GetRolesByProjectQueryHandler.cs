namespace Modules.Core.Services.Query.DataQueryHandlers
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Query;
	using Common.Security;
	using Infrastructure.AD;
	using Modules.Core.Contracts.UI.Dto;
	using Modules.Core.Services.Query.DataQueries;
	using Modules.Core.Services.UI.Renderers;

	[UsedImplicitly]
	internal sealed class GetRolesByProjectQueryHandler : IDataQueryHandler<GetRolesByProjectQuery, UserRoleDto[]>
	{
		private readonly IRoleProvider _roleProvider;

		private readonly ISolutionGroupManager _solutionGroupManager;

		private readonly IUserAuthorityValidator _userAuthorityValidator;

		private readonly IUserPrincipal _userPrincipal;

		public GetRolesByProjectQueryHandler([NotNull] IRoleProvider roleProvider,
			[NotNull] ISolutionGroupManager solutionGroupManager,
			[NotNull] IUserAuthorityValidator userAuthorityValidator,
			[NotNull] IUserPrincipal userPrincipal)
		{
			if (roleProvider == null) throw new ArgumentNullException(nameof(roleProvider));
			if (solutionGroupManager == null) throw new ArgumentNullException(nameof(solutionGroupManager));
			if (userAuthorityValidator == null) throw new ArgumentNullException(nameof(userAuthorityValidator));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_roleProvider = roleProvider;
			_solutionGroupManager = solutionGroupManager;
			_userAuthorityValidator = userAuthorityValidator;
			_userPrincipal = userPrincipal;
		}

		public UserRoleDto[] Execute([NotNull] GetRolesByProjectQuery dataQuery)
		{
			if (dataQuery == null) throw new ArgumentNullException(nameof(dataQuery));

			if (!_userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.Settings.ViewAccessControl
				},
				dataQuery.ProjectId))
				throw new UnauthorizedAccessException();

			var roles = _roleProvider.Get(dataQuery.ProjectId);

			var spec = new RoleRenderer().GetSpec();

			var userRolesDto = roles.Select(_ => spec.Invoke(_)).ToArray();

			foreach (var userRole in userRolesDto)
			{
				var groupInfo = _solutionGroupManager.GetBySid(userRole.Sid);

				userRole.GroupName = groupInfo.GroupName;
			}

			return userRolesDto.ToArray();
		}
	}
}