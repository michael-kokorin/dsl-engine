namespace Infrastructure.Tests.DataSource
{
	using System;
	using System.Data.Entity.Infrastructure;
	using System.Linq;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Enums;
	using Common.Security;
	using Infrastructure.DataSource;
	using Repository.Context;
	using Repository.Repositories;

	[TestFixture]
	public sealed class DataSourceAccessValidatorTest
	{
		private IDataSourceAccessValidator _target;

		private Mock<IDataSourceAuthorityNameBuilder> _dataSourceAuthorityNameBuilder;

		private Mock<ITableRepository> _tableRepository;

		private Mock<IUserAuthorityValidator> _userAuthorityValidator;

		[SetUp]
		public void SetUp()
		{
			_dataSourceAuthorityNameBuilder = new Mock<IDataSourceAuthorityNameBuilder>();

			_tableRepository = new Mock<ITableRepository>();

			_userAuthorityValidator = new Mock<IUserAuthorityValidator>();

			_target = new DataSourceAccessValidator(
				_dataSourceAuthorityNameBuilder.Object,
				_tableRepository.Object,
				_userAuthorityValidator.Object);
		}

		[Test]
		public void ShouldGrantAccessToDataSource()
		{
			const int dataSourceId = 234;
			const int userId = 5323;

			const string dataSourceName = "Test";

			const long projectId = 123;

			_dataSourceAuthorityNameBuilder
				.Setup(_ => _.GetDataSourceAuthorityName(dataSourceName))
				.Returns("read_datasource_Test");

			_userAuthorityValidator.Setup(
				_ => _.GetProjects(
					userId,
					new[]
					{
						"read_datasource_Test"
					})).Returns(
				new[]
				{
					projectId
				});

			_tableRepository
				.Setup(_ => _.GetAvailable(dataSourceId))
				.Returns(
					new[]
					{
						new Tables
						{
							Type = (int) DataSourceType.User,
							Name = dataSourceName
						}
					}.AsQueryable());

			var result = _target.CanReadSource(dataSourceId, userId);

			_userAuthorityValidator.Verify(
				_ => _.GetProjects(
					userId,
					new[]
					{
						"read_datasource_Test"
					}),
				Times.Once);

			result.Should().BeTrue();
		}

		[Test]
		public void ShouldNotGrantAccessToClossedDataSources()
		{
			const int dataSourceId = 234;
			const int userId = 5323;

			_tableRepository
				.Setup(_ => _.GetAvailable(dataSourceId))
				.Returns(
					new[]
					{
						new Tables
						{
							Type = (int) DataSourceType.Closed
						}
					}.AsQueryable());

			var result = _target.CanReadSource(dataSourceId, userId);

			result.Should().BeFalse();
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ShouldThrowDataSourceDoesNotExistsException()
		{
			const int dataSourceId = 234;
			const int userId = 5323;

			_tableRepository
				.Setup(_ => _.GetById(dataSourceId))
				.Returns((Tables) null);

			var result = _target.CanReadSource(dataSourceId, userId);

			result.Should().BeFalse();
		}
	}
}