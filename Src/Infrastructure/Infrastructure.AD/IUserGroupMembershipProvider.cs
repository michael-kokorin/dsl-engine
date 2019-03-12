namespace Infrastructure.AD
{
	using System.Collections.Generic;

	public interface IUserGroupMembershipProvider
	{
		IEnumerable<string> GetUsersByGroup(string groupSid);

		bool IsInGroup(string userSid, string groupSid);
	}
}