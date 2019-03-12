namespace Infrastructure.AD.Tests
{
	using System;
	using System.DirectoryServices.AccountManagement;
	using System.Linq;
	using System.Security.Principal;

	using FluentAssertions;

	using NUnit.Framework;

	[TestFixture]
	public sealed class AdTest
	{
		private static void ValidateIdentity(WindowsIdentity identity)
		{
			Assert.IsNotNull(identity);

			var principal = new WindowsPrincipal(identity);

			var groupSid = GetUserFirstGroupSid(principal);

			UserShouldBeInRole(principal, groupSid);

			GroupExists(groupSid);
		}

		private static void GroupExists(string groupSid)
		{
			var group = GetGroup(groupSid);

			@group.Should().NotBeNull();
		}

		private static void UserShouldBeInRole(IPrincipal principal, string groupSid)
		{
			var result = principal.IsInRole(groupSid);

			result.ShouldBeEquivalentTo(true);
		}

		private static string GetUserFirstGroupSid(WindowsPrincipal principal)
		{
			var gSid =
				principal.UserClaims
					.Where(_ =>
						(_.Type == @"http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid") &&
						(_.Value.Length > 10))
					.Select(_ => _.Value)
					.FirstOrDefault();

			gSid.Should().NotBeNullOrEmpty();
			return gSid;
		}

		private static GroupPrincipal GetGroup(string id)
		{
			var ctx = new PrincipalContext(ContextType.Domain);

			return GroupPrincipal.FindByIdentity(ctx, id);
		}

		[Test]
		public void ShouldTestCurrentUser()
		{
			using (var identity = WindowsIdentity.GetCurrent())
			{
				ValidateIdentity(identity);
			}
		}

		[Test]
		public void ShouldTestCurrentUserByPrincipal()
		{
			using (var identity = WindowsIdentity.GetCurrent())
			{
				Assert.IsNotNull(identity);

				var principal = new WindowsPrincipal(identity);

				var groupSid = GetUserFirstGroupSid(principal);

				principal.IsInRole(groupSid).Should().BeTrue();
			}
		}

		[Test]
		public void ShouldTestUserByName()
		{
			using (var perantIdentity = WindowsIdentity.GetCurrent())
			{
				Assert.IsNotNull(perantIdentity);

				var userName =
					perantIdentity.Name.Substring(perantIdentity.Name.IndexOf(@"\", StringComparison.InvariantCulture) +
					                              1);

				using (var identity = new WindowsIdentity(userName))
				{
					ValidateIdentity(identity);
				}
			}
		}
	}
}