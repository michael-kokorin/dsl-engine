namespace Infrastructure.AD
{
	using System;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Repository.Context;
	using Repository.Repositories;

	internal sealed class GroupNameBuilder : IGroupNameBuilder
	{
		private const string KeyFormat = "{0}_{1}";

		private readonly IProjectRepository _projectRepository;

		private readonly IRoleRepository _roleRepository;

		public GroupNameBuilder(
			[NotNull] IProjectRepository projectRepository,
			[NotNull] IRoleRepository roleRepository)
		{
			if (projectRepository == null) throw new ArgumentNullException(nameof(projectRepository));
			if (roleRepository == null) throw new ArgumentNullException(nameof(roleRepository));

			_projectRepository = projectRepository;
			_roleRepository = roleRepository;
		}

		public GroupNameInfo Build(string roleAlias, string roleName, long? projectId = null)
		{
			if (string.IsNullOrEmpty(roleAlias))
				throw new ArgumentNullException(nameof(roleAlias));

			if (string.IsNullOrEmpty(roleName))
				throw new ArgumentNullException(nameof(roleName));

			if (projectId == null)
				return new GroupNameInfo
				{
					Name = roleAlias,
					Description = Resources.Resources.SdlRole.FormatWith(roleName)
				};

			var project = GetProject(projectId);

			return new GroupNameInfo
			{
				Name = KeyFormat.FormatWith(project.Alias, roleAlias),
				Description = Resources.Resources.SdlRoleInProject.FormatWith(roleName, project.DisplayName)
			};
		}

		public GroupNameInfo Build(long roleId, long? projectId = null)
		{
			var role = GetRoleAlias(roleId);

			return Build(role.Alias, role.DisplayName, projectId);
		}

		private Roles GetRoleAlias(long roleId)
		{
			var role = _roleRepository.GetById(roleId);

			if (role == null)
				throw new ArgumentException(nameof(roleId));

			return role;
		}

		private Projects GetProject(long? projectId)
		{
			if (projectId == null) return null;

			var project = _projectRepository.GetById(projectId.Value);

			if (project == null)
				throw new ArgumentException(nameof(projectId));

			return project;
		}
	}
}