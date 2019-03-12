namespace Infrastructure.Tests.DataSource
{
	using System.Linq;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.DataSource;
	using Repository.Context;
	using Repository.Repositories;

	[TestFixture]
	public sealed class DataSourceInfoProviderTest
	{
		private DataSourceInfoProvider _target;

		private Mock<IDataSourceAccessValidator> _dataSourceAccessValidator;

		private Mock<ITableRepository> _tableRepository;

		[SetUp]
		public void SetUp()
		{
			_dataSourceAccessValidator = new Mock<IDataSourceAccessValidator>();

			_tableRepository = new Mock<ITableRepository>();

			_target = new DataSourceInfoProvider(_dataSourceAccessValidator.Object, _tableRepository.Object);
		}

		private const long UserId = 5235;

		[Test]
		public void ShouldReturnEmptySourcesList()
		{
			var sources = _target.Get(UserId).ToArray();

			sources.Length.ShouldBeEquivalentTo(0);
		}

		[Test]
		public void ShouldNotReturnSourcesWhenEmpty()
		{
			var sources = _target.Get(UserId).ToArray();

			sources.Length.ShouldBeEquivalentTo(0);
		}

		[Test]
		public void ShouldReturnDataSource()
		{
			const long dataSourceId = 523234;

			_tableRepository
				.Setup(_ => _.GetAvailable())
				.Returns(new[]
				{
					new Tables
					{
						Id = dataSourceId
					}
				}.AsQueryable());

			_dataSourceAccessValidator
				.Setup(_ => _.CanReadSource(dataSourceId, UserId))
				.Returns(true);

			var sources = _target.Get(UserId).ToArray();

			sources.Length.ShouldBeEquivalentTo(1);

			sources[0].Id.ShouldBeEquivalentTo(dataSourceId);
		}

		[Test]
		public void ShouldNotReturnDataSource()
		{
			const long dataSourceId = 523234;

			_tableRepository
				.Setup(_ => _.LocalizedQuery())
				.Returns(new[]
				{
					new Tables
					{
						Id = dataSourceId
					}
				}.AsQueryable());

			_dataSourceAccessValidator
				.Setup(_ => _.CanReadSource(dataSourceId, UserId))
				.Returns(false);

			var sources = _target.Get(UserId).ToArray();

			sources.Length.ShouldBeEquivalentTo(0);
		}
	}
}