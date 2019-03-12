namespace Infrastructure.AD
{
	using System;

	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class SolutionGroupManager : ISolutionGroupManager
	{
		private readonly IActiveDirectoryClient _activeDirectoryClient;

		private readonly IActiveDirectoryPathProvider _activeDirectoryPathProvider;

		public SolutionGroupManager([NotNull] IActiveDirectoryClient activeDirectoryClient,
			[NotNull] IActiveDirectoryPathProvider activeDirectoryPathProvider)
		{
			if (activeDirectoryClient == null) throw new ArgumentNullException(nameof(activeDirectoryClient));
			if (activeDirectoryPathProvider == null) throw new ArgumentNullException(nameof(activeDirectoryPathProvider));

			_activeDirectoryClient = activeDirectoryClient;
			_activeDirectoryPathProvider = activeDirectoryPathProvider;
		}

		public void AddUser(string groupSid, string userSid)
		{
			var adPath = _activeDirectoryPathProvider.GetPath();

			if (_activeDirectoryClient.IsInGroup(adPath, userSid, groupSid))
				return;

			_activeDirectoryClient.Add(adPath, userSid, groupSid);
		}

		public AdGroupInfo Create(string groupName, string groupDescription = null)
		{
			var adPath = _activeDirectoryPathProvider.GetPath();

			return _activeDirectoryClient.CreateGroup(adPath, groupName, groupDescription);
		}

		public AdGroupInfo GetByName(string groupName)
		{
			var adPath = _activeDirectoryPathProvider.GetPath();

			return _activeDirectoryClient.GetGroup(adPath, groupName);
		}

		public AdGroupInfo GetBySid(string groupSid)
		{
			var adPath = _activeDirectoryPathProvider.GetPath();

			return _activeDirectoryClient.GetGroupBySid(adPath, groupSid);
		}
	}
}