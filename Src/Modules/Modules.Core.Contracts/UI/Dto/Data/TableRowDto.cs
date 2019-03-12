namespace Modules.Core.Contracts.UI.Dto.Data
{
    using System.Runtime.Serialization;

    [DataContract(Name = "TableRow")]
    public sealed class TableRowDto
    {
        [DataMember]
        public TableRowItemDto[] Items { get; set; }

        [DataMember]
        public long? EntityId { get; set; }
    }
}