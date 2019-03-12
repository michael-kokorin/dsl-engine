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
	internal sealed class GetRolesByUserQueryHandler : IDataQueryHandler<GetRolesByUserQuery, UserRoleDto[]>
	{
		private readonly ISolutionGroupManager _solutionGroupManager;

		private readonly IUserPrincipal _userPrincipal;

		private readonly IUserProvider _userProvider;

		private readonly IUserRoleProvider _userRoleProvider;

		public GetRolesByUserQueryHandler([NotNull] ISolutionGroupManager solutionGroupManager,
			[NotNull] IUserPrincipal userPrincipal,
			[NotNull] IUserProvider userProvider,
			[NotNull] IUserRoleProvider userRoleProvider)
		{
			if (solutionGroupManager == null) throw new ArgumentNullException(nameof(solutionGroupManager));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));
			if (userProvider == null) throw new ArgumentNullException(nameof(userProvider));
			if (userRoleProvider == null) throw new ArgumentNullException(nameof(userRoleProvider));

			_solutionGroupManager = solutionGroupManager;
			_userPrincipal = userPrincipal;
			_userProvider = userProvider;
			_userRoleProvider = userRoleProvider;
		}

		public UserRoleDto[] Execute(GetRolesByUserQuery dataQuery)
		{
			// can view only own roles list
			if (dataQuery.UserId != _userPrincipal.Info.Id)
				throw new UnauthorizedAccessException();

			var user = _userProvider.Get(dataQuery.UserId);

			var roles = _userRoleProvider.GetUserRoles(user);

			var userroles = roles
				.Select(new RoleRenderer().GetSpec())
				.ToArray();

			foreach (var userRole in userroles)
			{
				var groupInfo = _solutionGroupManager.GetBySid(userRole.Sid);

				userRole.GroupName = groupInfo.GroupName;
			}

			return userroles;
		}
	}
}