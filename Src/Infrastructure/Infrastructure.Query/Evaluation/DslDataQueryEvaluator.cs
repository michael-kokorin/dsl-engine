// ReSharper disable PossibleMultipleEnumeration
namespace Infrastructure.Query.Evaluation
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.DataSource;
	using Infrastructure.Engines;
	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Query;
	using Infrastructure.Engines.Query.Result;
	using Infrastructure.Query.Evaluation.Exceptions;

	[UsedImplicitly]
	internal sealed class DslDataQueryEvaluator : IDslDataQueryEvaluator
	{
		private readonly IDataSourceFieldInfoProvider _dataSourceFieldInfoProvider;

		private readonly IDataSourceInfoProvider _dataSourceInfoProvider;

		private readonly IQueryEntityNameTranslator _queryEntityNameTranslator;

		private readonly IQueryProjectRestrictor _queryProjectRestrictor;

		private readonly IQueryVariableNameBuilder _queryVariableNameBuilder;

		private readonly IFormatBlockValueAccessEvaluator _formatBlockValueAccessEvaluator;

		public DslDataQueryEvaluator([NotNull] IDataSourceFieldInfoProvider dataSourceFieldInfoProvider,
			[NotNull] IDataSourceInfoProvider dataSourceInfoProvider,
			[NotNull] IQueryEntityNameTranslator queryEntityNameTranslator,
			[NotNull] IQueryProjectRestrictor queryProjectRestrictor,
			[NotNull] IQueryVariableNameBuilder queryVariableNameBuilder,
			[NotNull] IFormatBlockValueAccessEvaluator formatBlockValueAccessEvaluator)
		{
			if (dataSourceFieldInfoProvider == null) throw new ArgumentNullException(nameof(dataSourceFieldInfoProvider));
			if (dataSourceInfoProvider == null) throw new ArgumentNullException(nameof(dataSourceInfoProvider));
			if (queryEntityNameTranslator == null) throw new ArgumentNullException(nameof(queryEntityNameTranslator));
			if (queryProjectRestrictor == null) throw new ArgumentNullException(nameof(queryProjectRestrictor));
			if (queryVariableNameBuilder == null) throw new ArgumentNullException(nameof(queryVariableNameBuilder));
			if (formatBlockValueAccessEvaluator == null)
				throw new ArgumentNullException(nameof(formatBlockValueAccessEvaluator));

			_dataSourceFieldInfoProvider = dataSourceFieldInfoProvider;
			_dataSourceInfoProvider = dataSourceInfoProvider;
			_queryEntityNameTranslator = queryEntityNameTranslator;
			_queryProjectRestrictor = queryProjectRestrictor;
			_queryVariableNameBuilder = queryVariableNameBuilder;
			_formatBlockValueAccessEvaluator = formatBlockValueAccessEvaluator;
		}

		public IEnumerable<QueryResultColumn> Evaluate(DslDataQuery query, long userId)
		{
			if (query == null)
				throw new ArgumentNullException(nameof(query));

			var entityType = _queryEntityNameTranslator.GetEntityType(query.QueryEntityName);

			var dataSource = _dataSourceInfoProvider.Get(entityType.Name, userId);

			if (dataSource == null)
				return Enumerable.Empty<QueryResultColumn>();

			var isQueryBlocksIsEmpty = (query.Blocks == null) ||
			                           !query.Blocks.Any();

			if (isQueryBlocksIsEmpty)
			{
				return GetDefaultColumns(query, userId, dataSource);
			}

			RestrictFirstSelect(query, dataSource, userId);

			_queryProjectRestrictor.Restrict(query, entityType, dataSource, userId);

			return GetLastSelectColumns(query);
		}

		private IEnumerable<QueryResultColumn> GetDefaultColumns([NotNull] DslDataQuery query,
			long userId,
			DataSourceInfo dataSource)
		{
			if (query == null) throw new ArgumentNullException(nameof(query));

			var availableFields = _dataSourceFieldInfoProvider.GetBySource(dataSource.Id, userId);

			var dslFormatBlock = CreateFormatExprFromFields(availableFields);

			query.Blocks = new IDslQueryBlock[] {dslFormatBlock};

			var columns = availableFields.Select(_ => new QueryResultColumn
			{
				Code = _.Key,
				Description = _.Description,
				Name = _.Name
			});

			return columns;
		}

		private void RestrictFirstSelect(DslDataQuery query, DataSourceInfo dataSource, long userId)
		{
			var firstFormatBlock = GetFirstFormatBlock(query);

			ProcessSelects(dataSource, userId, firstFormatBlock);
		}

		private void ProcessSelects(DataSourceInfo dataSource, long userId, DslFormatBlock firstFormatBlock)
		{
			foreach (var selectItem in firstFormatBlock.Selects)
			{
				var selectItemProperty = _queryVariableNameBuilder.Decode(selectItem.Value);

				ProcessSelectItem(dataSource, userId, selectItem, selectItemProperty);
			}
		}

		private void ProcessSelectItem(DataSourceInfo dataSource,
			long userId,
			DslFormatItem selectItem,
			string selectItemProperty)
		{
			if (string.IsNullOrEmpty(selectItem.Name))
				selectItem.Name = selectItemProperty;

			var isValueField = _queryVariableNameBuilder.IsSimpleValue(selectItemProperty);

			if (!isValueField)
			{
				if (string.IsNullOrEmpty(selectItem.Name))
					throw new SelectFieldNameEmptyException(selectItem.Value);
			}

			DataSourceFieldInfo inheritedField;

			var isCanRequestColumn = _formatBlockValueAccessEvaluator.IsAccessible(
				selectItemProperty,
				dataSource.Key,
				userId,
				out inheritedField);

			if (!isCanRequestColumn)
			{
				selectItem.Value = QueryKey.QueryEmptyString;

				return;
			}

			if (inheritedField == null)
				return;

			SetSelectItemInfoFromField(selectItem, inheritedField);
		}

		private static void SetSelectItemInfoFromField(DslFormatItem formatItem, DataSourceFieldInfo field)
		{
			if (string.IsNullOrEmpty(formatItem.DisplayName))
				formatItem.DisplayName = field.Name;

			if (string.IsNullOrEmpty(formatItem.Description))
				formatItem.Description = field.Description;
		}

		private static DslFormatBlock GetFirstFormatBlock(DslDataQuery query) =>
			query.Blocks.OfType<DslFormatBlock>().First();

		private static IEnumerable<QueryResultColumn> GetLastSelectColumns(DslDataQuery query)
		{
			var lastFormatBlock = query.Blocks.OfType<DslFormatBlock>().Last();

			return lastFormatBlock.Selects.Select(_ => new QueryResultColumn
			{
				Code = _.Name,
				Name = _.DisplayName ?? _.Name,
				Description = _.Description
			});
		}

		private DslFormatBlock CreateFormatExprFromFields(IEnumerable<DataSourceFieldInfo> availableFields) =>
			new DslFormatBlock
			{
				Selects = availableFields.Select(_ => new DslFormatItem
				{
					Value = _queryVariableNameBuilder.Encode(_.Key)
				})
			};
	}
}