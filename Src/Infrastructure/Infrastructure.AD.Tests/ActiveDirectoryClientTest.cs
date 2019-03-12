namespace Infrastructure.AD.Tests
{
	using System;
	using System.Collections;
	using System.DirectoryServices;
	using System.Linq;
	using System.Security.Principal;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Logging;

	[TestFixture]
	[Ignore("Integration tests")]
	public sealed class ActiveDirectoryClientTest
	{
		[SetUp]
		public void SetUp() => _target = new ActiveDirectoryClient(new Mock<ILog>().Object);

		private IActiveDirectoryClient _target;

		[Test]
		public void ShouldCreateLocalGroup()
		{
			var path = "WinNT://" + Environment.MachineName + ",computer";

			const string groupName = "Test";

			var imptCtxt = new WrapperImpersonationContext("MSHARONOV", "sdl.core", "P@ssw0rd");

			imptCtxt.Enter();

			var result = _target.CreateGroup(path, groupName).Sid;

			imptCtxt.Leave();

			result.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void ShouldResolveCurrentUser()
		{
			var currentIdentity = WindowsIdentity.GetCurrent();

			Assert.IsNotNull(currentIdentity);

			var path = "WinNT://" + Environment.MachineName;

			const string groupName = "Administrator";

			var groupSid = _target.GetGroup(path, groupName).Sid;

			groupSid.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void ShouldGetUserMembershipByDirectoryEntry()
		{
			var path = "WinNT://" + Environment.MachineName;

			var result = _target.IsInGroup(path, "S-1-5-21-1023191730-727829927-3985050192-22364", "S-1-5-21-543419179-2457612372-1216098091-1012");

			result.Should().BeTrue();
		}

		[Test]
		public void ShouldGetLocalGroupMembers()
		{
			const string groupName = "Administrator";

			var path = "WinNT://" + Environment.MachineName;

			var searchRoot = new DirectoryEntry(path);

			var grp = searchRoot.Children.Find(groupName);

			var members = grp.Invoke("members", null);

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var groupMember in (IEnumerable)members)
			{
				var member = new DirectoryEntry(groupMember);

				if (member.SchemaClassName != "User") continue;

				var sid = GetEntrySid(member);

				sid.Should().NotBeNullOrEmpty();
			}
		}

		[Test]
		public void ShouldGetLocalGroupMembersBySid()
		{
			const string groupName = "Administrator";

			var path = "WinNT://" + Environment.MachineName;

			var searchRoot = new DirectoryEntry(path);

			var grp = searchRoot.Children.Find(groupName);

			var members = grp.Invoke("members", null);

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var groupMember in (IEnumerable)members)
			{
				var member = new DirectoryEntry(groupMember);

				if (member.SchemaClassName != "User") continue;

				var sid = GetEntrySid(member);

				sid.Should().NotBeNullOrEmpty();
			}
		}

		private static string GetEntrySid(DirectoryEntry @group)
		{
			var groupSidBinary = @group.Properties["objectSid"][0] as byte[];

			if (groupSidBinary == null)
				return null;

			var groupSid = new SecurityIdentifier(groupSidBinary, 0);

			return groupSid.Value;
		}

		[Test]
		public void ShouldGetUsersByGroup()
		{
			var path = "WinNT://" + Environment.MachineName;

			const string groupName = "Администраторы";

			var groupSid = _target.GetGroup(path, groupName).Sid;

			var users = _target.GetUsersByGroup(groupSid).ToArray();

			users.Should().NotBeNull();
		}

		[Test]
		public void ShouldGetUsersByPathGroup()
		{
			var path = "WinNT://" + Environment.MachineName;

			const string groupName = "Администраторы";

			var groupSid = _target.GetGroup(path, groupName).Sid;

			var users = _target.GetUsersByGroup(path, groupSid).ToArray();

			users.Should().NotBeNull();
		}
	}
}