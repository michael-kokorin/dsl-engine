namespace Infrastructure.Query.Tests
{
	using System;

	using Moq;

	using NUnit.Framework;

	using Common.Security;
	using Infrastructure.DataSource;
	using Infrastructure.Engines.Dsl.Query;

	[TestFixture]
	public sealed class QueryModelAccessValidatorTest
	{
		private IQueryModelAccessValidator _target;

		private Mock<IDataSourceInfoProvider> _dataSourceInfoProvider;

		private Mock<IDataSourceAccessValidator> _dataSourceAccessValidator;

		private Mock<IUserPrincipal> _userPrincipal;

		[SetUp]
		public void SetUp()
		{
			_dataSourceInfoProvider = new Mock<IDataSourceInfoProvider>();

			_dataSourceAccessValidator = new Mock<IDataSourceAccessValidator>();

			_userPrincipal = new Mock<IUserPrincipal>();

			_target = new QueryModelAccessValidator(
				_dataSourceInfoProvider.Object,
				_dataSourceAccessValidator.Object,
				_userPrincipal.Object);
		}

		[Test]
		public void ShouldNotThrowExceptionWhenDataSourceDoesNotExists()
		{
			const string entityName = "entity";

			var query = new DslDataQuery
			{
				QueryEntityName = entityName
			};

			const long userId = 523243;

			long? projectId = 423434;

			var userInfo = new UserInfo
			{
				Id = userId
			};

			_userPrincipal.Setup(_ => _.Info).Returns(userInfo);

			_dataSourceInfoProvider.Setup(_ => _.Get(entityName, userId)).Returns((DataSourceInfo) null);

			_target.Validate(query, projectId, false);
		}

		[Test]
		public void ShouldNotThrowExceptionWhenHaventAccessToDataSource()
		{
			const string entityName = "entity";

			var query = new DslDataQuery
			{
				QueryEntityName = entityName
			};

			const long userId = 523243;

			var userInfo = new UserInfo
			{
				Id = userId
			};

			_userPrincipal.Setup(_ => _.Info).Returns(userInfo);

			const long dataSourceId = 23443234;

			var dataSource = new DataSourceInfo
			{
				Id = dataSourceId
			};

			long? projectId = 52123123;

			_dataSourceInfoProvider
				.Setup(_ => _.Get(entityName, userId))
				.Returns(dataSource);

			_dataSourceAccessValidator
				.Setup(_ => _.CanReadSource(dataSourceId, userId))
				.Returns(false);

			Assert.Throws<UnauthorizedAccessException>(() => _target.Validate(query, projectId, false));
		}
	}
}