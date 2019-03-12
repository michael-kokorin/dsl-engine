namespace Infrastructure.Query.Tests
{
	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Enums;
	using Common.Security;
	using Repository.Context;

	using Authorities = Common.Security.Authorities;

	[TestFixture]
	public sealed class QueryAccessValidatorTest
	{
		private IQueryAccessValidator _target;

		private Mock<IUserAuthorityValidator> _userAuthorityValidator;

		[SetUp]
		public void SetUp()
		{
			_userAuthorityValidator = new Mock<IUserAuthorityValidator>();

			_target = new QueryAccessValidator(_userAuthorityValidator.Object);
		}

		[Test]
		public void ShouldCanViewOwnQuery()
		{
			const long userId = 123;

			const int queryId = 5235;

			const long projectId = 234;

			var query = new Queries
			{
				CreatedById = userId,
				Id = queryId,
				ProjectId = projectId,
				Privacy = (int) QueryPrivacyType.Private
			};

			_userAuthorityValidator.Setup(_ => _.HasUserAuthorities(
				userId,
				new[] {Authorities.UI.Queries.ViewQuery},
				projectId))
				.Returns(true);

			var result = _target.IsCanView(query, userId);

			result.Should().BeTrue();
		}

		[Test]
		public void ShouldCantViewPrivateQuery()
		{
			const long userId = 123;

			const int queryId = 5235;

			const long projectId = 234;

			var query = new Queries
			{
				CreatedById = 312,
				Id = queryId,
				ProjectId = projectId,
				Privacy = (int)QueryPrivacyType.Private
			};

			_userAuthorityValidator.Setup(_ => _.HasUserAuthorities(
				userId,
				new[] { Authorities.UI.Queries.ViewQuery },
				projectId))
				.Returns(true);

			var result = _target.IsCanView(query, userId);

			result.Should().BeFalse();
		}

		[Test]
		public void ShouldCanEditOwnQuery()
		{
			const long userId = 123;

			const int queryId = 5235;

			var query = new Queries
			{
				CreatedById = userId,
				Id = queryId
			};

			var result = _target.IsCanEdit(query, userId);

			result.Should().BeTrue();
		}

		[Test]
		public void ShouldCanViewOtherUserQueryWhenCanReadAllQueries()
		{
			const long userId = 123;
			const long createdById = 53245;
			const long projectId = 23235235234;

			const int queryId = 5235;

			var query = new Queries
			{
				CreatedById = createdById,
				Id = queryId,
				ProjectId = projectId
			};

			_userAuthorityValidator.Setup(_ => _.HasUserAuthorities(
				userId,
				new[] {Authorities.UI.Queries.ViewQueriesAll},
				projectId))
				.Returns(true);

			var result = _target.IsCanView(query, userId);

			result.Should().BeTrue();
		}

		[Test]
		public void ShouldCanEditOtherUserQueryWhenCanEditAllQueries()
		{
			const long userId = 123;
			const long createdById = 53245;
			const long projectId = 23235235234;

			const int queryId = 5235;

			var query = new Queries
			{
				CreatedById = createdById,
				Id = queryId,
				ProjectId = projectId
			};

			_userAuthorityValidator.Setup(_ => _.HasUserAuthorities(
				userId,
				new[] {Authorities.UI.Queries.EditQueryAll},
				projectId))
				.Returns(true);

			var result = _target.IsCanEdit(query, userId);

			result.Should().BeTrue();
		}

		[Test]
		public void ShouldCanViewOtherUserQueryWhenQueryIsPublic()
		{
			const long userId = 123;
			const long createdById = 53245;
			const long projectId = 23235235234;

			const int queryId = 5235;

			var query = new Queries
			{
				CreatedById = createdById,
				Id = queryId,
				Privacy = (int) QueryPrivacyType.PublicRead,
				ProjectId = projectId
			};

			_userAuthorityValidator.Setup(_ => _.HasUserAuthorities(
				userId,
				new[] {Authorities.UI.Queries.ViewQuery},
				projectId))
				.Returns(true);

			var result = _target.IsCanView(query, userId);

			result.Should().BeTrue();
		}

		[Test]
		public void ShouldCanEditOtherUserQueryWhenQueryTypeIsPublicWrite()
		{
			const long userId = 123;
			const long createdById = 53245;
			const long projectId = 23235235234;

			const int queryId = 5235;

			var query = new Queries
			{
				CreatedById = createdById,
				Id = queryId,
				Privacy = (int) QueryPrivacyType.PublicWrite,
				ProjectId = projectId
			};

			_userAuthorityValidator.Setup(_ => _.HasUserAuthorities(
				userId,
				new[] {Authorities.UI.Queries.EditQuery},
				projectId))
				.Returns(true);

			var result = _target.IsCanEdit(query, userId);

			result.Should().BeTrue();
		}

		[Test]
		public void ShouldNotCanViewOtherUserQueryWhenQueryIsPrivate()
		{
			const long userId = 123;
			const long createdById = 53245;
			const long projectId = 23235235234;

			const int queryId = 5235;

			var query = new Queries
			{
				CreatedById = createdById,
				Id = queryId,
				Privacy = (int) QueryPrivacyType.Private,
				ProjectId = projectId
			};

			_userAuthorityValidator.Setup(_ => _.HasUserAuthorities(
				userId,
				new[] {Authorities.UI.Queries.ViewQuery},
				projectId))
				.Returns(true);

			var result = _target.IsCanView(query, userId);

			result.Should().BeFalse();
		}

		[Test]
		public void ShouldNotCanEditOtherUserQueryWhenQueryTypeIsPrivate()
		{
			const long userId = 123;
			const long createdById = 53245;
			const long projectId = 23235235234;

			const int queryId = 5235;

			var query = new Queries
			{
				CreatedById = createdById,
				Id = queryId,
				Privacy = (int) QueryPrivacyType.Private,
				ProjectId = projectId
			};

			_userAuthorityValidator.Setup(_ => _.HasUserAuthorities(
				userId,
				new[] {Authorities.UI.Queries.EditQuery},
				projectId))
				.Returns(true);

			var result = _target.IsCanEdit(query, userId);

			result.Should().BeFalse();
		}

		[Test]
		public void ShouldNotCanEditOtherUserQueryWhenQueryTypeIsPublicRead()
		{
			const long userId = 123;
			const long createdById = 53245;
			const long projectId = 23235235234;

			const int queryId = 5235;

			var query = new Queries
			{
				CreatedById = createdById,
				Id = queryId,
				Privacy = (int) QueryPrivacyType.PublicRead,
				ProjectId = projectId
			};

			_userAuthorityValidator.Setup(_ => _.HasUserAuthorities(
				userId,
				new[] {Authorities.UI.Queries.EditQuery},
				projectId))
				.Returns(true);

			var result = _target.IsCanEdit(query, userId);

			result.Should().BeFalse();
		}
	}
}