namespace Infrastructure.Engines
{
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.AD;
	using Infrastructure.Engines.Dsl;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
	internal sealed class UserProvider : IUserProvider
	{
		private readonly IRoleRepository _roleRepository;
		private readonly IUserRepository _userRepository;

		private readonly IUserRoleProvider _userRoleProvider;

		public UserProvider(
			IUserRepository userRepository,
			IRoleRepository roleRepository,
			IUserRoleProvider userRoleProvider)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
			_userRoleProvider = userRoleProvider;
		}

		public IEnumerable<Users> GetUsers(SubjectsExpr subjects, long? projectId)
		{
			var users = new List<Users>();

			var hasSpecificRoles = (subjects.Roles != null) && (subjects.Roles.Length != 0);

			if (subjects.IsAll || hasSpecificRoles)
			{
				var roles = _roleRepository.GetByProject(projectId);

				if (hasSpecificRoles)
				{
					var selectedRoles = subjects.Roles.Select(x => x.RoleName).ToArray();
					roles = roles.Where(x => selectedRoles.Any(y => y == x.Alias));
				}

				var usersFromRoles = new List<long>();

				foreach (var role in roles)
				{
					var roleUsers = _userRoleProvider.GetUsersByRole(role).Select(_ => _.Id);

					usersFromRoles.AddRange(roleUsers);
				}

				var distinctUsers = usersFromRoles.Distinct().ToArray();
				var actualUsers = _userRepository.Query().Where(x => distinctUsers.Any(y => y == x.Id)).ToArray();
				if (hasSpecificRoles)
				{
					var excludedPersons =
						subjects.Roles.Where(x => (x.ExcludedPersons != null) && (x.ExcludedPersons.Length != 0))
							.SelectMany(x => x.ExcludedPersons).Distinct().ToArray();
					actualUsers = actualUsers.Where(x => excludedPersons.All(y => y != x.Login)).ToArray();
				}

				users.AddRange(actualUsers);
			}

			if ((subjects.Persons == null) || (subjects.Persons.Length == 0))
				return users.Distinct().ToArray();

			var persons = _userRepository.Query().Where(x => subjects.Persons.Any(y => y == x.Login)).ToArray();
			users.AddRange(persons);

			return users.Distinct().ToArray();
		}
	}
}