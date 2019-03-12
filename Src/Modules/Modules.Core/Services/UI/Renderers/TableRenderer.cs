// ReSharper disable PossibleMultipleEnumeration
namespace Modules.Core.Services.UI.Renderers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Infrastructure.Engines.Query.Result;
	using Modules.Core.Contracts.UI.Dto.Data;
	using Modules.Core.Extensions;

	internal sealed class TableRenderer
	{
		public TableDto Render(QueryResult queryResult)
		{
			if (queryResult == null)
				throw new ArgumentNullException(nameof(queryResult));

			var table = Render(queryResult.Items);

			FillColumnData(queryResult, table);

			FillExceptionData(queryResult, table);

			return table;
		}

		private static void FillExceptionData(QueryResult queryResult, TableDto table)
		{
			if (queryResult.Exceptions == null ||
			    !queryResult.Exceptions.Any())
				return;

			table.Exceptions = queryResult
				.Exceptions
				.Select(_ => new QueryExceptionDto
				{
					Message = _.Message
				})
				.ToArray();
		}

		private static void FillColumnData(QueryResult queryResult, TableDto table)
		{
			if (queryResult.Columns == null ||
				!queryResult.Columns.Any())
				return;

			foreach (var dataColumn in queryResult.Columns)
			{
				var tableColumn = table.Columns.SingleOrDefault(c => c.Key == dataColumn.Code);

				if (tableColumn == null)
				{
					tableColumn = new TableColumnDto
					{
						Key = dataColumn.Code,
						Order = table.Columns.Any()
							? table.Columns.Max(_ => _.Order) + 1
							: 1
					};

					var columnsList = table.Columns.ToList();

					columnsList.Add(tableColumn);

					table.Columns = columnsList.ToArray();
				}

				tableColumn.Description = dataColumn.Description;

				tableColumn.Name = dataColumn.Name;
			}
		}

		public TableDto Render(IEnumerable<QueryResultItem> collection)
		{
			if (collection == null
				|| !collection.Any())
				return new TableDto();

			var properties = GetProperties(collection.First().Value);

			var table = new TableDto { Columns = GetColumns(properties) };

			if (table.Columns.Length == 0)
				return table;

			table.Rows = GetRows(collection);

			return table;
		}

		private static PropertyInfo[] GetProperties(object item) =>
			item.GetType().GetProperties();

		private static TableColumnDto[] GetColumns(IReadOnlyCollection<PropertyInfo> properties)
		{
			if (properties.Count == 0)
				return new TableColumnDto[0];

			var columns = new List<TableColumnDto>();

			var order = 1;

			foreach (var property in properties)
			{
				columns.Add(new TableColumnDto
				{
					Description = property.Localize() ?? property.Name,
					Key = property.Name,
					Name = property.Localize() ?? property.Name,
					Order = order
				});

				order++;
			}

			return columns.ToArray();
		}

		private TableRowDto[] GetRows(IEnumerable<QueryResultItem> collection)
		{
			if (collection == null) throw new ArgumentNullException(nameof(collection));

			if (!collection.Any())
				return new TableRowDto[0];

			var properties = GetProperties(collection.First().Value);

			return (
				from item in collection
				select new TableRowDto
				{
					EntityId = item.EntityId,
					Items = properties.Select(p => new TableRowItemDto
					{
						ColumnKey = p.Name,
						Value = item.Value
							.GetType()
							.GetProperty(p.Name)
							.GetValue(item.Value, null)
					}).ToArray()
				}).ToArray();
		}
	}
}