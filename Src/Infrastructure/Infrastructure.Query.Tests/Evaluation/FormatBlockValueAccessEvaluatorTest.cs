namespace Infrastructure.Query.Tests.Evaluation
{
	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.DataSource;
	using Infrastructure.Engines.Query;
	using Infrastructure.Query.Evaluation;

	[TestFixture]
	public sealed class FormatBlockValueAccessEvaluatorTest
	{
		private Mock<IDataSourceAccessValidator> _dataSourceAccessValidator;

		private Mock<IDataSourceFieldInfoProvider> _dataSourceFieldInfoProvider;

		private Mock<IQueryVariableNameBuilder> _queryVariableNameBuilder;

		private Mock<IQueryEntityNamePropertyTypeNameResolver> _queryEntityNamePropertyTypeNameResolver;

		private IFormatBlockValueAccessEvaluator _target;

		[SetUp]
		public void SetUp()
		{
			_dataSourceAccessValidator = new Mock<IDataSourceAccessValidator>();
			_dataSourceFieldInfoProvider = new Mock<IDataSourceFieldInfoProvider>();
			_queryVariableNameBuilder = new Mock<IQueryVariableNameBuilder>();
			_queryEntityNamePropertyTypeNameResolver = new Mock<IQueryEntityNamePropertyTypeNameResolver>();

			_target = new FormatBlockValueAccessEvaluator(
				_dataSourceAccessValidator.Object,
				_dataSourceFieldInfoProvider.Object,
				_queryEntityNamePropertyTypeNameResolver.Object,
				_queryVariableNameBuilder.Object);
		}

		[Test]
		public void ShouldReturnTrueWhenHaveAccessToLocalField()
		{
			const string expression = "Name.ToString()";
			const string currentDataSource = "datasource";
			const int userId = 234;

			_dataSourceAccessValidator.Setup(_ => _.CanReadSource(currentDataSource, userId)).Returns(true);

			_queryVariableNameBuilder.Setup(_ => _.IsSimpleValue("Name")).Returns(true);
			_queryVariableNameBuilder.Setup(_ => _.IsSimpleValue("ToString()")).Returns(false);

			var fieldInfo = new DataSourceFieldInfo();

			_dataSourceFieldInfoProvider
				.Setup(_ => _.TryGet(currentDataSource, "Name", userId))
				.Returns(fieldInfo);

			DataSourceFieldInfo resultFieldInfo;

			var result = _target.IsAccessible(expression, currentDataSource, userId, out resultFieldInfo);

			result.Should().BeTrue();
		}

		[Test]
		public void ShouldReturnTrueWhenHaveAccessToForeignTableField()
		{
			const string expression = "Projects.Name.ToString()";
			const string currentDataSource = "datasource";
			const int userId = 234;

			_dataSourceAccessValidator.Setup(_ => _.CanReadSource(currentDataSource, userId)).Returns(true);
			_dataSourceAccessValidator.Setup(_ => _.CanReadSource("Projects", userId)).Returns(true);

			_queryEntityNamePropertyTypeNameResolver
				.Setup(_ => _.ResolvePropertyTypeName(currentDataSource, "Projects"))
				.Returns("Projects");

			_queryVariableNameBuilder.Setup(_ => _.IsSimpleValue("Projects")).Returns(true);
			_queryVariableNameBuilder.Setup(_ => _.IsSimpleValue("Name")).Returns(true);
			_queryVariableNameBuilder.Setup(_ => _.IsSimpleValue("ToString()")).Returns(false);

			var fieldInfo = new DataSourceFieldInfo();

			_dataSourceFieldInfoProvider
				.Setup(_ => _.TryGet("Projects", "Name", userId))
				.Returns(fieldInfo);

			DataSourceFieldInfo resultFieldInfo;

			var result = _target.IsAccessible(expression, currentDataSource, userId, out resultFieldInfo);

			result.Should().BeTrue();
		}

		[Test]
		public void ShouldReturnFalseWhenHaveNotAccessToForeignTableField()
		{
			const string expression = "Projects.Name.ToString()";
			const string currentDataSource = "datasource";
			const int userId = 234;

			_dataSourceAccessValidator.Setup(_ => _.CanReadSource(currentDataSource, userId)).Returns(true);
			_dataSourceAccessValidator.Setup(_ => _.CanReadSource("Projects", userId)).Returns(false);

			_queryEntityNamePropertyTypeNameResolver
				.Setup(_ => _.ResolvePropertyTypeName(currentDataSource, "Projects"))
				.Returns("Projects");

			_queryVariableNameBuilder.Setup(_ => _.IsSimpleValue("Projects")).Returns(true);
			_queryVariableNameBuilder.Setup(_ => _.IsSimpleValue("Name")).Returns(true);
			_queryVariableNameBuilder.Setup(_ => _.IsSimpleValue("ToString()")).Returns(false);

			DataSourceFieldInfo resultFieldInfo;

			var result = _target.IsAccessible(expression, currentDataSource, userId, out resultFieldInfo);

			result.Should().BeFalse();
		}
	}
}