// ReSharper disable PossibleMultipleEnumeration
namespace Infrastructure.AD
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Security;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class AdUserAuthorityValidator : IUserAuthorityValidator
	{
		private readonly IProjectRepository _projectRepository;

		private readonly IRoleRepository _roleRepository;

		private readonly IUserGroupMembershipProvider _userGroupMembershipProvider;

		private readonly IUserRepository _userRepository;

		public AdUserAuthorityValidator([NotNull] IProjectRepository projectRepository,
			[NotNull] IRoleRepository roleRepository,
			[NotNull] IUserGroupMembershipProvider userGroupMembershipProvider,
			[NotNull] IUserRepository userRepository)
		{
			if (projectRepository == null) throw new ArgumentNullException(nameof(projectRepository));
			if (roleRepository == null) throw new ArgumentNullException(nameof(roleRepository));
			if (userGroupMembershipProvider == null) throw new ArgumentNullException(nameof(userGroupMembershipProvider));
			if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));

			_projectRepository = projectRepository;
			_roleRepository = roleRepository;
			_userGroupMembershipProvider = userGroupMembershipProvider;
			_userRepository = userRepository;
		}

		public bool HasUserAuthorities(long userId, IEnumerable<string> authorityNames, long? projectId)
		{
			var userSid = GetUserSid(userId);

			if (string.IsNullOrEmpty(userSid))
				throw new ArgumentException(nameof(userSid));

			var groupSids = GetRolesByAuthorities(projectId, authorityNames.ToArray());

			return groupSids.Any(groupSid => _userGroupMembershipProvider.IsInGroup(userSid, groupSid));
		}

		public IEnumerable<long> GetProjects(long userId, IEnumerable<string> authorityNames)
		{
			var userSid = GetUserSid(userId);

			if (string.IsNullOrEmpty(userSid))
				yield break;

			var groups = _roleRepository.Get(authorityNames).ToArray();

			var globalGroups = groups.Where(_ => _.ProjectId == null);

			if (globalGroups.Any(_ => _userGroupMembershipProvider.IsInGroup(userSid, _.Sid)))
			{
				var projectIds = _projectRepository.Query().Select(_ => _.Id);

				foreach (var projectId in projectIds)
				{
					yield return projectId;
				}

				yield break;
			}

			var perProjectGroups = groups
				.Where(_ => _.ProjectId != null)
				.ToLookup(_ => _.ProjectId.Value);

			foreach (var projectGroups in perProjectGroups)
			{
				// ReSharper disable once LoopCanBeConvertedToQuery
				foreach (var projectGroup in projectGroups)
				{
					// ReSharper disable once InvertIf
					if (_userGroupMembershipProvider.IsInGroup(userSid, projectGroup.Sid))
					{
						yield return projectGroups.Key;

						break;
					}
				}
			}
		}

		private string GetUserSid(long userId) => _userRepository
			.Get(userId)
			.Select(_ => _.Sid)
			.SingleOrDefault();

		private IEnumerable<string> GetRolesByAuthorities(long? projectId, params string[] authorityNames) =>
			_roleRepository
				.GetByAuthorities(authorityNames, projectId)
				.Select(_ => _.Sid)
				.Distinct()
				.ToArray();
	}
}