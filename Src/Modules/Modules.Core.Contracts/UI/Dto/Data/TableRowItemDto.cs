namespace Modules.Core.Contracts.UI.Dto.Data
{
    using System.Runtime.Serialization;

    [DataContract(Name = "TableRowItem")]
    public sealed class TableRowItemDto
    {
        [DataMember]
        public string ColumnKey { get; set; }

        [DataMember]
        public object Value { get; set; }
    }
}