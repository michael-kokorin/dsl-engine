namespace Infrastructure.AD
{
	using System.Collections.Generic;

	// ReSharper disable once MemberCanBeInternal
	public interface IActiveDirectoryClient
	{
		/// <summary>
		/// Creates the group in specified AD/SAM path.
		/// </summary>
		/// <param name="path">The AD/SAM path.</param>
		/// <param name="groupName">Name of the AD/SAM group.</param>
		/// <param name="groupDescription">The group description.</param>
		/// <returns>AS/SAM group SID</returns>
		AdGroupInfo CreateGroup(string path, string groupName, string groupDescription = null);

		/// <summary>
		/// Gets the users by group.
		/// </summary>
		/// <param name="groupSid">The AD/SAM group SID.</param>
		/// <returns>Array of AD/SAM user SID</returns>
		IEnumerable<string> GetUsersByGroup(string groupSid);

		/// <summary>
		/// Gets the users by group in specified path.
		/// </summary>
		/// <param name="path">The AD/SAM path.</param>
		/// <param name="groupSid">The AD/SAM group SID.</param>
		/// <returns>Array of AD/SAM user SID</returns>
		IEnumerable<string> GetUsersByGroup(string path, string groupSid);

		/// <summary>
		/// Determines whether [is user in group] [the specified user sid].
		/// </summary>
		/// <param name="userSid">The AD user SID.</param>
		/// <param name="groupSid">The AD group SID.</param>
		/// <returns>User in specified group</returns>
		bool IsUserInGroup(string userSid, string groupSid);


		/// <summary>
		/// Determines whether [is in group] [the specified path].
		/// </summary>
		/// <param name="path">The AD path.</param>
		/// <param name="userSid">The AD user SID.</param>
		/// <param name="groupSid">The AD group SID.</param>
		/// <returns>User belongs to roup in specified path</returns>
		bool IsInGroup(string path, string userSid, string groupSid);


		/// <summary>
		/// Adds user to group
		/// </summary>
		/// <param name="path">The AD path.</param>
		/// <param name="userSid">The AD user SID.</param>
		/// <param name="groupSid">The AD group SID.</param>
		void Add(string path, string userSid, string groupSid);

		/// <summary>
		/// Gets the AD group SID.
		/// </summary>
		/// <param name="path">The AD path.</param>
		/// <param name="groupName">Name of the group.</param>
		/// <returns>AD Group information</returns>
		AdGroupInfo GetGroup(string path, string groupName);

		/// <summary>
		/// Gets the group by SID.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="groupSid">The group sid.</param>
		/// <returns>AD Group information</returns>
		AdGroupInfo GetGroupBySid(string path, string groupSid);
	}
}