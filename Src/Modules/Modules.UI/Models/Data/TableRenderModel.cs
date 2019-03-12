namespace Modules.UI.Models.Data
{
    using System;

    public sealed class TableRenderModel
    {
        public TableModel Table { get; set; }

        public string TargetController { get; set; }

        public string TargetAction { get; set; }

        public Func<TableRowModel, object> ArgumentSetter { get; set; }

        public string RowIconClassName { get; set; }

        public string TableName { get; set; }

        public TableRenderModel()
        {
            RowIconClassName = "glyphicon glyphicon-object-align-bottom";

            TableName = "tableMain";
        }
    }
}