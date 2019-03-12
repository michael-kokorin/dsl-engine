namespace Modules.Core.Contracts.UI.Dto.Data
{
    using System.Runtime.Serialization;

    [DataContract(Name = "TableColumn")]
    public sealed class TableColumnDto
    {
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Order { get; set; }
    }
}