namespace Infrastructure.AD
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.DirectoryServices;
	using System.DirectoryServices.AccountManagement;
	using System.DirectoryServices.ActiveDirectory;
	using System.Linq;
	using System.Security.Principal;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Logging;

	internal sealed class ActiveDirectoryClient : IActiveDirectoryClient
	{
		private readonly ILog _log;

		public ActiveDirectoryClient([NotNull] ILog log)
		{
			if (log == null) throw new ArgumentNullException(nameof(log));

			_log = log;
		}

		public AdGroupInfo CreateGroup(string path, string groupName, string groupDescription = null)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			if (string.IsNullOrEmpty(groupName))
				throw new ArgumentNullException(nameof(groupName));

			string groupSid;

			using (var entry = GetEntiry(path))
			{
				using (var group = entry.Children.Add(groupName, "group"))
				{
					if (!string.IsNullOrEmpty(groupDescription))
						group.Properties["description"].Add(groupDescription);

					group.CommitChanges();

					groupSid = GetEntrySid(group);
				}
			}

			_log.Debug(
				Resources.Resources.ActiveDirectoryClient_CreateGroup_GroupCreated.FormatWith(
					groupName,
					path,
					groupSid));

			return new AdGroupInfo
			{
				GroupName = groupSid,
				Sid = groupSid
			};
		}

		public IEnumerable<string> GetUsersByGroup(string groupSid)
		{
			if (string.IsNullOrEmpty(groupSid))
				throw new ArgumentNullException(nameof(groupSid));

			var group = GetGroup(groupSid);

			return @group?.GetMembers(true).Select(_ => _.Sid.Value) ?? Enumerable.Empty<string>();
		}

		public IEnumerable<string> GetUsersByGroup(string path, string groupSid)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			if (string.IsNullOrEmpty(groupSid))
				throw new ArgumentNullException(nameof(groupSid));

			using (var entry = GetEntiry(path))
			{
				using (var grp = entry.Children.Find(GetEntryName(groupSid), "group"))
				{
					var members = (IEnumerable) grp.Invoke("members", null);

					if (members == null)
						yield break;

					// ReSharper disable once LoopCanBePartlyConvertedToQuery
					foreach (var groupMember in members)
					{
						var member = new DirectoryEntry(groupMember);

						if (member.SchemaClassName != "User") continue;

						var sid = GetEntrySid(member);

						yield return sid;
					}
				}
			}
		}

		public bool IsUserInGroup(string userSid, string groupSid)
		{
			var group = GetGroup(groupSid);

			var user = GetUser(userSid);

			return user.IsMemberOf(group);
		}

		public bool IsInGroup(string path, string userSid, string groupSid)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			if (string.IsNullOrEmpty(userSid))
				throw new ArgumentNullException(nameof(userSid));

			if (string.IsNullOrEmpty(groupSid))
				throw new ArgumentNullException(nameof(groupSid));

			using (var entry = GetEntiry(path))
			{
				using (var grp = entry.Children.Find(GetEntryName(groupSid), "group"))
				{
					var members = (IEnumerable) grp.Invoke("members", null);

					if (members == null)
						return false;

					// ReSharper disable once LoopCanBeConvertedToQuery
					foreach (var groupMember in members)
					{
						var member = new DirectoryEntry(groupMember);

						if (member.SchemaClassName != "User") continue;

						var sid = GetEntrySid(member);

						if (sid == userSid) return true;
					}
				}
			}

			return false;
		}

		public void Add(string path, string userSid, string groupSid)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			if (string.IsNullOrEmpty(userSid))
				throw new ArgumentNullException(nameof(userSid));

			if (string.IsNullOrEmpty(groupSid))
				throw new ArgumentNullException(nameof(groupSid));

			using (var ctxt = GetEntiry(path))
			{
				using (var grp = ctxt.Children.Find(GetEntryName(groupSid), "group"))
				{
					var usrName = "WinNT://" + GetAccountName(userSid).Replace('\\', '/') + ", user";

					using (var usr = GetEntiry(usrName))
					{
						grp.Invoke("Add", usr.Path);

						grp.CommitChanges();
					}
				}
			}
		}

		public AdGroupInfo GetGroup(string path, string groupName)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			if (string.IsNullOrEmpty(groupName))
				throw new ArgumentNullException(nameof(groupName));

			using (var entry = GetEntiry(path))
			{
				using (var group = entry.Children.Find(groupName, "group"))
				{
					return new AdGroupInfo
					{
						Sid = GetEntrySid(group),
						GroupName = group.Name
					};
				}
			}
		}

		public AdGroupInfo GetGroupBySid(string path, string groupSid)
		{
			using (var ctxt = GetEntiry(path))
			{
				using (var grp = ctxt.Children.Find(GetEntryName(groupSid), "group"))
				{
					return new AdGroupInfo
					{
						GroupName = grp.Name,
						Sid = groupSid
					};
				}
			}
		}

		private static string GetEntryName(string entrySid)
		{
			var nt = GetAccountName(entrySid);

			return nt.Split('\\')[1];
		}

		private static string GetAccountName(string entrySid)
		{
			var sid = new SecurityIdentifier(entrySid);

			var nt = sid.Translate(typeof (NTAccount)).Value;
			return nt;
		}

		private static string GetEntrySid(DirectoryEntry @group)
		{
			var groupSidBinary = @group.Properties["objectSid"][0] as byte[];

			if (groupSidBinary == null)
				throw new ActiveDirectoryOperationException();

			var groupSid = new SecurityIdentifier(groupSidBinary, 0);

			return groupSid.Value;
		}

		private static DirectoryEntry GetEntiry(string path) => new DirectoryEntry(path);

		private static PrincipalContext GetContext(ContextType type)
		{
			switch (type)
			{
				case ContextType.Machine:
					return new PrincipalContext(type, Environment.MachineName);
				case ContextType.Domain:
					return new PrincipalContext(type, Environment.UserDomainName);
				case ContextType.ApplicationDirectory:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}

			throw new NotSupportedException(nameof(type));
		}

		private static UserPrincipal GetUser(PrincipalContext context, string userSid)
			=> UserPrincipal.FindByIdentity(context, IdentityType.Sid, userSid);

		private static UserPrincipal GetUser(string sid)
		{
			var localUser = GetUser(GetContext(ContextType.Machine), sid);

			return localUser ?? GetUser(GetContext(ContextType.Domain), sid);
		}

		private static GroupPrincipal GetGroup(PrincipalContext context, string groupSid)
			=> GroupPrincipal.FindByIdentity(context, IdentityType.Sid, groupSid);

		private static GroupPrincipal GetGroup(string sid)
		{
			var localUser = GetGroup(GetContext(ContextType.Machine), sid);

			return localUser ?? GetGroup(GetContext(ContextType.Domain), sid);
		}
	}
}