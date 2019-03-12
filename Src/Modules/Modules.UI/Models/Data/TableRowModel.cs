namespace Modules.UI.Models.Data
{
    using System.Collections.Generic;

    public sealed class TableRowModel
    {
        public IEnumerable<TableRowItemModel> Items { get; set; }

        public long? EntityId { get; set; }
    }
}