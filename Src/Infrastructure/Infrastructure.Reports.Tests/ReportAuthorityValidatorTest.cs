namespace Infrastructure.Reports.Tests
{
	using System.Collections.Generic;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Security;
	using Repository.Context;

	using Authorities = Common.Security.Authorities;

	[TestFixture]
	public sealed class ReportAuthorityValidatorTest
	{
		private IReportAuthorityValidator _target;

		private Mock<IUserAuthorityValidator> _userAuthorityValidator;

		[SetUp]
		public void SetUp()
		{
			_userAuthorityValidator = new Mock<IUserAuthorityValidator>();

			_target = new ReportAuthorityValidator(_userAuthorityValidator.Object);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void ShouldValidateReportCreateAuthority(bool isHasAuthority)
		{
			const long userId = 234234;

			long? projectId = 4234;

			_userAuthorityValidator
				.Setup(_ => _.HasUserAuthorities(userId, new[] {Authorities.UI.Reports.Create}, projectId))
				.Returns(isHasAuthority);

			var result = _target.CanCreate(userId, projectId);

			result.ShouldBeEquivalentTo(isHasAuthority);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void ShouldValidateReportEditAuthority(bool isHasAuthority)
		{
			const long userId = 4234;

			long? projectId = 4233123;

			var report = new Reports
			{
				IsSystem = false,
				ProjectId = projectId
			};

			_userAuthorityValidator
				.Setup(_ => _.HasUserAuthorities(userId, new[] {Authorities.UI.Reports.Edit}, projectId))
				.Returns(isHasAuthority);

			var result = _target.CanEdit(userId, report);

			result.ShouldBeEquivalentTo(isHasAuthority);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void ShouldValidateReportRunAuthority(bool isHasAuthority)
		{
			const long userId = 4234;

			long? projectId = 4233123;

			var report = new Reports
			{
				IsSystem = false,
				ProjectId = projectId
			};

			_userAuthorityValidator
				.Setup(_ => _.HasUserAuthorities(userId, new[] { Authorities.UI.Reports.Run }, projectId))
				.Returns(isHasAuthority);

			var result = _target.CanRun(userId, report);

			result.ShouldBeEquivalentTo(isHasAuthority);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void ShouldValidateReportViewAuthority(bool isHasAuthority)
		{
			const long userId = 4234;

			long? projectId = 4233123;

			var report = new Reports
			{
				IsSystem = false,
				ProjectId = projectId
			};

			_userAuthorityValidator
				.Setup(_ => _.HasUserAuthorities(userId, new[] { Authorities.UI.Reports.View }, projectId))
				.Returns(isHasAuthority);

			var result = _target.CanView(userId, report);

			result.ShouldBeEquivalentTo(isHasAuthority);
		}

		[TestCase(new[] {3L, 6L}, true)]
		[TestCase(new long[] {}, false)]
		public void ShouldValidateReportEditAuthorityForSystemReport(IEnumerable<long> projectIds, bool available)
		{
			const long userId = 4234;

			var report = new Reports
			{
				IsSystem = true,
				ProjectId = null
			};

			const string authorityName = Authorities.UI.Reports.Edit;

			_userAuthorityValidator
				.Setup(_ => _.HasUserAuthorities(userId, new[] {authorityName}, null))
				.Returns(false);

			_userAuthorityValidator
				.Setup(_ => _.GetProjects(userId, new[] {authorityName}))
				.Returns(projectIds);

			var result = _target.CanEdit(userId, report);

			result.ShouldBeEquivalentTo(available);
		}

		[TestCase(new[] {3L, 6L}, true)]
		[TestCase(new long[] {}, false)]
		public void ShouldValidateReportRunAuthorityForSystemReport(IEnumerable<long> projectIds, bool available)
		{
			const long userId = 4234;

			var report = new Reports
			{
				IsSystem = true,
				ProjectId = null
			};

			const string authorityName = Authorities.UI.Reports.Run;

			_userAuthorityValidator
				.Setup(_ => _.HasUserAuthorities(userId, new[] {authorityName}, null))
				.Returns(false);

			_userAuthorityValidator
				.Setup(_ => _.GetProjects(userId, new[] {authorityName}))
				.Returns(projectIds);

			var result = _target.CanRun(userId, report);

			result.ShouldBeEquivalentTo(available);
		}

		[TestCase(new[] { 3L, 6L }, true)]
		[TestCase(new long[] { }, false)]
		public void ShouldValidateReportViewAuthorityForSystemReport(IEnumerable<long> projectIds, bool available)
		{
			const long userId = 4234;

			var report = new Reports
			{
				IsSystem = true,
				ProjectId = null
			};

			const string authorityName = Authorities.UI.Reports.View;

			_userAuthorityValidator
				.Setup(_ => _.HasUserAuthorities(userId, new[] { authorityName }, null))
				.Returns(false);

			_userAuthorityValidator
				.Setup(_ => _.GetProjects(userId, new[] { authorityName }))
				.Returns(projectIds);

			var result = _target.CanView(userId, report);

			result.ShouldBeEquivalentTo(available);
		}
	}
}