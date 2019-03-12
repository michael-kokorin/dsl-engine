namespace Infrastructure.Tests.DataSource
{
	using System;
	using System.Linq;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Enums;
	using Infrastructure.DataSource;
	using Repository.Context;
	using Repository.Repositories;

	[TestFixture]
	public sealed class DataSourceFieldAccessValidatorTest
	{
		private IDataSourceFieldAccessValidator _target;

		private Mock<IDataSourceAccessValidator> _dataSourceAccessValidator;

		private Mock<ITableColumnsRepository> _tableColumnsRepository;

		[SetUp]
		public void SetUp()
		{
			_dataSourceAccessValidator = new Mock<IDataSourceAccessValidator>();
			_tableColumnsRepository = new Mock<ITableColumnsRepository>();

			_target = new DataSourceFieldAccessValidator(
				_dataSourceAccessValidator.Object,
				_tableColumnsRepository.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ShouldThrowDataSourceFieldDoesNotExistsException()
		{
			const int fieldId = 234;
			const int userId = 5323;

			_tableColumnsRepository
				.Setup(_ => _.GetById(fieldId))
				.Returns((TableColumns)null);

			var result = _target.CanReadSourceField(fieldId, userId);

			result.Should().BeFalse();
		}

		[Test]
		public void ShouldNotGrantAccessToClossedDataSourceFields()
		{
			const int fieldId = 234;
			const int userId = 5323;

			_tableColumnsRepository
				.Setup(_ => _.GetAvailable(fieldId))
				.Returns(
					new[]
					{
						new TableColumns
						{
							FieldType = (int) DataSourceFieldType.Closed
						}
					}.AsQueryable());

			var result = _target.CanReadSourceField(fieldId, userId);

			result.Should().BeFalse();
		}

		[Test]
		public void ShouldGrantAccessToDataSourceField()
		{
			const int fieldId = 234;
			const int userId = 5323;

			const string fieldName = "Test";

			const long dataSourceId = 123;
			const string dataSourceName = "Source";

			_tableColumnsRepository
				.Setup(_ => _.GetAvailable(fieldId))
				.Returns(new[]
				{
					new TableColumns
				{
					Type = (int) DataSourceFieldType.User,
					Name = fieldName,
					TableId = dataSourceId,
					Tables1 = new Tables
					{
						Name = dataSourceName
					}
					}
				}.AsQueryable());

			_dataSourceAccessValidator
				.Setup(_ => _.CanReadSource(dataSourceId, userId))
				.Returns(true);

			var result = _target.CanReadSourceField(fieldId, userId);

			result.Should().BeTrue();
		}
	}
}