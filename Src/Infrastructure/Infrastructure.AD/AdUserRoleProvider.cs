namespace Infrastructure.AD
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class AdUserRoleProvider : IUserRoleProvider
	{
		private readonly IRoleRepository _roleRepository;

		private readonly IUserGroupMembershipProvider _userGroupMembershipProvider;

		private readonly IUserProjectSettingsRepository _userProjectSettingsRepository;

		private readonly IUserRepository _userRepository;

		public AdUserRoleProvider([NotNull] IRoleRepository roleRepository,
			[NotNull] IUserGroupMembershipProvider userGroupMembershipProvider,
			[NotNull] IUserProjectSettingsRepository userProjectSettingsRepository,
			[NotNull] IUserRepository userRepository)
		{
			if (roleRepository == null) throw new ArgumentNullException(nameof(roleRepository));
			if (userGroupMembershipProvider == null) throw new ArgumentNullException(nameof(userGroupMembershipProvider));
			if (userProjectSettingsRepository == null) throw new ArgumentNullException(nameof(userProjectSettingsRepository));
			if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));

			_roleRepository = roleRepository;
			_userGroupMembershipProvider = userGroupMembershipProvider;
			_userProjectSettingsRepository = userProjectSettingsRepository;
			_userRepository = userRepository;
		}

		public IEnumerable<Users> GetUsersByRole([NotNull] Roles role)
		{
			if (role == null) throw new ArgumentNullException(nameof(role));

			var userSids = _userGroupMembershipProvider.GetUsersByGroup(role.Sid);

			return userSids
				.Select(userSid => _userRepository.GetBySid(userSid).SingleOrDefault())
				.Where(user => user != null);
		}

		public Roles GetUserPreferedRoleForProject([NotNull] Users user, long projectId)
		{
			if (user == null) throw new ArgumentNullException(nameof(user));

			var userProjectSettings = _userProjectSettingsRepository.GetByUserAndProject(user.Id, projectId);

			if (userProjectSettings != null) return userProjectSettings.Roles;

			var roles = _roleRepository
				.GetByProject(projectId)
				.AsEnumerable();

			// ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
			var role = roles.Where(_ => IsUserInRole(user, _)).FirstOrDefault();

			if (role == null)
				throw new UnauthorizedAccessException(); // TODO: wtf?

			userProjectSettings = new UserProjectSettings
			{
				UserId = user.Id,
				PreferedRoleId = role.Id,
				ProjectId = projectId
			};

			_userProjectSettingsRepository.Insert(userProjectSettings);

			_userProjectSettingsRepository.Save();

			return role;
		}

		private bool IsUserInRole([NotNull] Users user, [NotNull] Roles role)
		{
			if (user == null) throw new ArgumentNullException(nameof(user));
			if (role == null) throw new ArgumentNullException(nameof(role));

			return _userGroupMembershipProvider.IsInGroup(user.Sid, role.Sid);
		}

		public IEnumerable<Roles> GetUserRoles([NotNull] Users user)
		{
			if (user == null) throw new ArgumentNullException(nameof(user));

			var roles = _roleRepository.GetActive().AsEnumerable();

			return roles.Where(_ => _userGroupMembershipProvider.IsInGroup(user.Sid, _.Sid));
		}
	}
}