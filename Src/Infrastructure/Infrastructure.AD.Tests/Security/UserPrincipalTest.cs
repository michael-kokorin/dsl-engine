namespace Infrastructure.AD.Tests.Security
{
	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Security;
	using Infrastructure.AD.Security;

	[TestFixture]
	public sealed class UserPrincipalTest
	{
		private IUserPrincipal _target;

		private Mock<ICurrentUserDataProvider> _userDataProvider;

		private const string UserSid = "user_sid";

		private readonly UserInfo _info = new UserInfo
		{
			Sid = UserSid
		};

		[SetUp]
		public void SetUp()
		{
			_userDataProvider = new Mock<ICurrentUserDataProvider>();

			_userDataProvider
				.Setup(_ => _.GetOrCreate())
				.Returns(_info);

			_target = new UserPrincipal(_userDataProvider.Object);
		}

		[Test]
		public void ShouldReturnUserInfo()
		{
			var result = _target.Info;

			result.ShouldBeEquivalentTo(_info);

			_userDataProvider.Verify(_ => _.GetOrCreate(), Times.Once);
		}

		[Test]
		public void ShouldReturnUserInfoOnce()
		{
			var result = _target.Info;

			result.ShouldBeEquivalentTo(_info);
			result.ShouldBeEquivalentTo(_info);

			_userDataProvider.Verify(_ => _.GetOrCreate(), Times.Once);
		}
	}
}