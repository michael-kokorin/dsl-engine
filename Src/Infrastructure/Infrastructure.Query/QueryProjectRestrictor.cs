namespace Infrastructure.Query
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.DataSource;
	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Dsl.Query.Filter;
	using Infrastructure.Engines.Dsl.Query.Filter.Specification;
	using Infrastructure.Engines.Query;
	using Repository;

	[UsedImplicitly]
	internal sealed class QueryProjectRestrictor : IQueryProjectRestrictor
	{
		private readonly IDataSourceAccessValidator _dataSourceAccessValidator;

		private readonly IQueryVariableNameBuilder _queryVariableNameBuilder;

		public QueryProjectRestrictor([NotNull] IDataSourceAccessValidator dataSourceAccessValidator,
			[NotNull] IQueryVariableNameBuilder queryVariableNameBuilder)
		{
			if (dataSourceAccessValidator == null) throw new ArgumentNullException(nameof(dataSourceAccessValidator));
			if (queryVariableNameBuilder == null) throw new ArgumentNullException(nameof(queryVariableNameBuilder));

			_dataSourceAccessValidator = dataSourceAccessValidator;
			_queryVariableNameBuilder = queryVariableNameBuilder;
		}

		public void Restrict([NotNull] DslDataQuery query,
			[NotNull] Type entityType,
			[NotNull] DataSourceInfo dataSource,
			long userId)
		{
			if (query == null) throw new ArgumentNullException(nameof(query));
			if (entityType == null) throw new ArgumentNullException(nameof(entityType));
			if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));

			var projectProperty = entityType
				.GetCustomAttributes(typeof(ProjectPropertyAttribute), false)
				.Cast<ProjectPropertyAttribute>()
				.SingleOrDefault();

			if (projectProperty == null)
				return;

			var userProjectIds = _dataSourceAccessValidator
				.GetDataSourceProjects(dataSource.Key, userId)
				.ToArray();

			if (!userProjectIds.Any())
				throw new UnauthorizedAccessException();

			var blocks = new List<IDslQueryBlock>();

			var propertyName = _queryVariableNameBuilder.Encode(projectProperty.PropertyName);

			var restrictWhere = new DslFilterBlock
			{
				Specification = new FilterSpecification
				{
					LeftSpecification = new FilterArraySpecification
					{
						Specifications = userProjectIds.Select(_ =>
							new FilterConstantSpecification
							{
								Value = $"{_}L"
							})
					},
					Operator = FilterOperator.Contains,
					RightSpecification = new FilterParameterSpecification
					{
						Value = propertyName
					}
				}
			};

			blocks.Add(restrictWhere);

			blocks.AddRange(query.Blocks);

			query.Blocks = blocks.ToArray();
		}
	}
}