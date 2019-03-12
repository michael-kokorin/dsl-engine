namespace Infrastructure.AD
{
	// ReSharper disable once MemberCanBeInternal
	public interface ISolutionGroupManager
	{
		AdGroupInfo Create(string groupAlias, string groupDescription = null);

		AdGroupInfo GetByName(string groupName);

		AdGroupInfo GetBySid(string groupSid);

		void AddUser(string groupSid, string userSid);
	}
}