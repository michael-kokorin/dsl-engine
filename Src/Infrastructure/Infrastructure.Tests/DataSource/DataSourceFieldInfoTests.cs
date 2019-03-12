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
	public sealed class DataSourceFieldInfoTests
	{
		private IDataSourceFieldInfoProvider _target;

		private Mock<IDataSourceFieldAccessValidator> _dataSourceFieldAccessValidator;

		private Mock<ITableColumnsRepository> _tableColumnsRepository;

		[SetUp]
		public void SetUp()
		{
			_dataSourceFieldAccessValidator = new Mock<IDataSourceFieldAccessValidator>();

			_tableColumnsRepository = new Mock<ITableColumnsRepository>();

			_target = new DataSourceFieldInfoProvider(
				_dataSourceFieldAccessValidator.Object,
				_tableColumnsRepository.Object);
		}

		[Test]
		public void ShouldReturnDataSourceField()
		{
			const long dataSourceId = 234;

			const long userId = 53;

			const long dataSourceFieldId = 5323;

			_tableColumnsRepository
				.Setup(_ => _.GetAvailableByTable(dataSourceId))
				.Returns(new[]
				{
					new TableColumns
					{
						Id = dataSourceFieldId
					}
				}.AsQueryable());

			_dataSourceFieldAccessValidator.Setup(_ => _.CanReadSourceField(dataSourceFieldId, userId)).Returns(true);

			var result = _target.GetBySource(dataSourceId, userId).ToArray();

			result.Length.ShouldBeEquivalentTo(1);

			result[0].Id.ShouldBeEquivalentTo(dataSourceFieldId);
		}

		[Test]
		public void ShouldNotReturnDataSourceField()
		{
			const long dataSourceId = 234;

			const long userId = 53;

			const long dataSourceFieldId = 5323;

			_tableColumnsRepository
				.Setup(_ => _.GetByTable(dataSourceId))
				.Returns(new[]
				{
					new TableColumns
					{
						Id = dataSourceFieldId
					}
				}.AsQueryable());

			_dataSourceFieldAccessValidator.Setup(_ => _.CanReadSourceField(dataSourceFieldId, userId)).Returns(false);

			var result = _target.GetBySource(dataSourceId, userId).ToArray();

			result.Length.ShouldBeEquivalentTo(0);
		}
	}
}