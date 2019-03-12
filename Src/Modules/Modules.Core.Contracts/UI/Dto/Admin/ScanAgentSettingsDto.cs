namespace Modules.Core.Contracts.UI.Dto.Admin
{
    using System.Runtime.Serialization;

    using Modules.Core.Contracts.UI.Dto.Data;

    [DataContract(Name = "ScanAgentSettings")]
    public sealed class ScanAgentSettingsDto
    {
        [DataMember]
        public TableDto ScanAgents { get; set; }
    }
}
