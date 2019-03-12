namespace Infrastructure.AD
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Common.Security;

	[UsedImplicitly]
	internal sealed class UserGroupMembershipProvider : IUserGroupMembershipProvider
	{
		private readonly IActiveDirectoryClient _activeDirectoryClient;

		private readonly IActiveDirectoryPathProvider _activeDirectoryPathProvider;

		private readonly IUserPrincipalProvider _userPrincipalProvider;

		public UserGroupMembershipProvider([NotNull] IActiveDirectoryClient activeDirectoryClient,
			[NotNull] IActiveDirectoryPathProvider activeDirectoryPathProvider,
			[NotNull] IUserPrincipalProvider userPrincipalProvider)
		{
			if (activeDirectoryClient == null) throw new ArgumentNullException(nameof(activeDirectoryClient));
			if (activeDirectoryPathProvider == null) throw new ArgumentNullException(nameof(activeDirectoryPathProvider));
			if (userPrincipalProvider == null) throw new ArgumentNullException(nameof(userPrincipalProvider));

			_activeDirectoryClient = activeDirectoryClient;
			_activeDirectoryPathProvider = activeDirectoryPathProvider;
			_userPrincipalProvider = userPrincipalProvider;
		}

		public IEnumerable<string> GetUsersByGroup(string groupSid) =>
			_activeDirectoryClient.GetUsersByGroup(_activeDirectoryPathProvider.GetPath(), groupSid);

		public bool IsInGroup(string userSid, string groupSid)
		{
			var path = _activeDirectoryPathProvider.GetPath();

			try
			{
				return _activeDirectoryClient.IsInGroup(path, userSid, groupSid);
			}
			catch
			{
				var isLocalUser = userSid.Equals(_userPrincipalProvider.Get().Sid,
					StringComparison.InvariantCultureIgnoreCase);

				// ReSharper disable once InvertIf
				if (isLocalUser)
				{
					var isInRoleByIdentity = _userPrincipalProvider.IsCurrentUserInRole(groupSid);

					if (isInRoleByIdentity)
						return true;
				}

				return _activeDirectoryClient.IsUserInGroup(userSid, groupSid);
			}
		}
	}
}