namespace Modules.UI.Models.Data
{
    using System.Collections.Generic;

    public sealed class TableModel
    {
        public IEnumerable<TableColumnModel> Columns { get; set; }

        public IEnumerable<TableRowModel> Rows { get; set; }
    }
}