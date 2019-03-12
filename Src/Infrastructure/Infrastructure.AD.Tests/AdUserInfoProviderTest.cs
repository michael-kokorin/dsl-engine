namespace Infrastructure.AD.Tests
{
	using System.Security.Principal;

	using FluentAssertions;

	using NUnit.Framework;

	[TestFixture]
	public sealed class AdUserInfoProviderTest
	{
		private IAdUserInfoProvider _target;

		[SetUp]
		public void SetUp() => _target = new AdUserInfoProvider();

		[Test]
		public void ShouldGetAdUserInfo()
		{
			var currentUserName = WindowsIdentity.GetCurrent().Name;

			currentUserName.Should().NotBeNullOrEmpty();

			var result = _target.Get(currentUserName);

			result.Should().NotBeNull();
		}


		[Test]
		[Ignore]
		public void ShouldResolveImpersonated()
		{
			var currentUserName = WindowsIdentity.GetCurrent().Name;

			var imptCtxt = new WrapperImpersonationContext("MSHARONOV", "sdl.core", "P@ssw0rd");

			imptCtxt.Enter();

			var impersonatedUserName = WindowsIdentity.GetCurrent().Name;

			impersonatedUserName.Should().NotStartWith(currentUserName);

			var result = _target.Get(impersonatedUserName);

			imptCtxt.Leave();

			result.Should().NotBeNull();
		}

		[Test]
		[Ignore]
		public void ShouldResolveDomainUser()
		{
			var currentUserName = WindowsIdentity.GetCurrent().Name;

			var imptCtxt = new WrapperImpersonationContext("MSHARONOV", "sdl.core", "P@ssw0rd");

			imptCtxt.Enter();

			System.AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

			var impersonatedUserName = WindowsIdentity.GetCurrent().Name;

			impersonatedUserName.Should().NotStartWith(currentUserName);

			var result = _target.Get(currentUserName);

			imptCtxt.Leave();

			result.Should().NotBeNull();
		}
	}
}