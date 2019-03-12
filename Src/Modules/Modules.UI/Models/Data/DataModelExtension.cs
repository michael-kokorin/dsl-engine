namespace Modules.UI.Models.Data
{
    using System.Linq;

    using Modules.Core.Contracts.UI.Dto.Data;

	internal static class DataModelExtension
    {
        public static TableModel ToModel(this TableDto table)
        {
            if (table == null)
                return null;

            return new TableModel
            {
                Columns = table.Columns?.Select(_ => _.ToModel()),
                Rows = table.Rows?.Select(_ => _.ToModel())
            };
        }

        private static TableColumnModel ToModel(this TableColumnDto column)
        {
            if (column == null)
                return null;

            return new TableColumnModel
            {
                Description = column.Description,
                Key = column.Key,
                Name = column.Name,
                Order = column.Order
            };
        }

        private static TableRowModel ToModel(this TableRowDto row)
        {
            if (row == null)
                return null;

            return new TableRowModel
            {
                EntityId = row.EntityId,
                Items = row.Items?.Select(_ => _.ToModel())
            };
        }

        private static TableRowItemModel ToModel(this TableRowItemDto item)
        {
            if (item == null)
                return null;

            return new TableRowItemModel
            {
                ColumnKey = item.ColumnKey,
                Value = item.Value
            };
        }
    }
}