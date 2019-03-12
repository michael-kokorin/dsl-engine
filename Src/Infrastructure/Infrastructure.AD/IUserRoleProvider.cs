namespace Infrastructure.AD
{
	using System.Collections.Generic;

	using Repository.Context;

	public interface IUserRoleProvider
	{
		IEnumerable<Users> GetUsersByRole(Roles role);

		Roles GetUserPreferedRoleForProject(Users user, long projectId);

		IEnumerable<Roles> GetUserRoles(Users user);
	}
}