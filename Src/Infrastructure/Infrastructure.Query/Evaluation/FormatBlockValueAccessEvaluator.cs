// ReSharper disable PossibleMultipleEnumeration

namespace Infrastructure.Query.Evaluation
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.DataSource;
	using Infrastructure.Engines.Query;

	[UsedImplicitly]
	internal sealed class FormatBlockValueAccessEvaluator : IFormatBlockValueAccessEvaluator
	{
		private readonly IDataSourceAccessValidator _dataSourceAccessValidator;

		private readonly IDataSourceFieldInfoProvider _dataSourceFieldInfoProvider;

		private readonly IQueryEntityNamePropertyTypeNameResolver _queryEntityNamePropertyTypeNameResolver;

		private readonly IQueryVariableNameBuilder _queryVariableNameBuilder;

		public FormatBlockValueAccessEvaluator([NotNull] IDataSourceAccessValidator dataSourceAccessValidator,
			[NotNull] IDataSourceFieldInfoProvider dataSourceFieldInfoProvider,
			[NotNull] IQueryEntityNamePropertyTypeNameResolver queryEntityNamePropertyTypeNameResolver,
			[NotNull] IQueryVariableNameBuilder queryVariableNameBuilder)
		{
			if (dataSourceAccessValidator == null) throw new ArgumentNullException(nameof(dataSourceAccessValidator));
			if (dataSourceFieldInfoProvider == null) throw new ArgumentNullException(nameof(dataSourceFieldInfoProvider));
			if (queryEntityNamePropertyTypeNameResolver == null)
				throw new ArgumentNullException(nameof(queryEntityNamePropertyTypeNameResolver));
			if (queryVariableNameBuilder == null) throw new ArgumentNullException(nameof(queryVariableNameBuilder));

			_dataSourceAccessValidator = dataSourceAccessValidator;
			_dataSourceFieldInfoProvider = dataSourceFieldInfoProvider;
			_queryEntityNamePropertyTypeNameResolver = queryEntityNamePropertyTypeNameResolver;
			_queryVariableNameBuilder = queryVariableNameBuilder;
		}

		private const char ElementSplitter = '.';

		public bool IsAccessible(string value, string dataSourceName, long userId, out DataSourceFieldInfo fieldInfo)
		{
			fieldInfo = null;

			var exprValues = GetSeparatedValues(value);

			var currentDataSourceName = dataSourceName;

			var lastSimpleValue = exprValues.LastOrDefault(_ => _queryVariableNameBuilder.IsSimpleValue(_));

			if (string.IsNullOrEmpty(lastSimpleValue))
			{
				return true;
			}

			foreach (var exprValue in exprValues)
			{
				var isSipleValue =  _queryVariableNameBuilder.IsSimpleValue(exprValue);

				if (!isSipleValue)
				{
					continue;
				}

				if (exprValue == lastSimpleValue)
					break;

				var entityName = _queryEntityNamePropertyTypeNameResolver.ResolvePropertyTypeName(
					currentDataSourceName,
					exprValue);

				var isCanReadCurrentDataSource = _dataSourceAccessValidator.CanReadSource(entityName, userId);

				if (!isCanReadCurrentDataSource)
					return false;

				currentDataSourceName = exprValue;
			}

			if (!string.IsNullOrEmpty(lastSimpleValue))
				fieldInfo = _dataSourceFieldInfoProvider.TryGet(currentDataSourceName, lastSimpleValue, userId);

			return true;
		}

		private static IEnumerable<string> GetSeparatedValues(string value) => value.Split(ElementSplitter);
	}
}