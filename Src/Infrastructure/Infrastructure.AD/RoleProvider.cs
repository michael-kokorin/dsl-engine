namespace Infrastructure.AD
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class RoleProvider : IRoleProvider
	{
		private readonly IGroupNameBuilder _groupNameBuilder;

		private readonly IRoleRepository _roleRepository;

		private readonly ISolutionGroupManager _solutionGroupManager;

		public RoleProvider([NotNull] IGroupNameBuilder groupNameBuilder,
			[NotNull] IRoleRepository roleRepository,
			[NotNull] ISolutionGroupManager solutionGroupManager)
		{
			if (groupNameBuilder == null) throw new ArgumentNullException(nameof(groupNameBuilder));
			if (roleRepository == null) throw new ArgumentNullException(nameof(roleRepository));
			if (solutionGroupManager == null) throw new ArgumentNullException(nameof(solutionGroupManager));

			_groupNameBuilder = groupNameBuilder;
			_roleRepository = roleRepository;
			_solutionGroupManager = solutionGroupManager;
		}

		public Roles Get(long? projectId, [NotNull] string alias)
		{
			if (alias == null) throw new ArgumentNullException(nameof(alias));

			if (string.IsNullOrEmpty(alias)) throw new ArgumentException(nameof(alias));

			return _roleRepository.Get(projectId, alias).SingleOrDefault();
		}

		public IEnumerable<Roles> Get(long? projectId) => _roleRepository.GetByProject(projectId);

		public long Create(string alias, string name, long tasksQueryId, long taskResultsQueryId, long? projectId = null)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			var role = Get(projectId, alias);

			if (role != null)
				return role.Id;

			role = new Roles
			{
				Alias = alias,
				DisplayName = name,
				ProjectId = projectId,
				TasksQueryId = tasksQueryId,
				TaskResultsQueryId = taskResultsQueryId
			};

			var group = TryCreateGroup(role);

			role.Sid = group.Sid;

			_roleRepository.Insert(role);

			_roleRepository.Save();

			return role.Id;
		}

		public void AddUser(long roleId, string userSid)
		{
			var role = _roleRepository.GetById(roleId);

			if (role == null)
				throw new ArgumentException(nameof(role));

			_solutionGroupManager.AddUser(role.Sid, userSid);
		}

		public AdGroupInfo TryCreateGroup(Roles role)
		{
			var groupNameInfo = _groupNameBuilder.Build(role.Alias, role.DisplayName, role.ProjectId);

			try
			{
				return _solutionGroupManager.GetByName(groupNameInfo.Name);
			}
			catch
			{
				return _solutionGroupManager.Create(groupNameInfo.Name, groupNameInfo.Description);
			}
		}
	}
}