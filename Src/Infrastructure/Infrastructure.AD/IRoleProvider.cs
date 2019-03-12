namespace Infrastructure.AD
{
	using System.Collections.Generic;

	using Repository.Context;

	public interface IRoleProvider
	{
		void AddUser(long roleId, string userSid);

		long Create(string alias, string name, long tasksQuery, long taskResultsQueryId, long? projectId = null);

		AdGroupInfo TryCreateGroup(Roles role);

		Roles Get(long? projectId, string alias);

		IEnumerable<Roles> Get(long? projectId);
	}
}