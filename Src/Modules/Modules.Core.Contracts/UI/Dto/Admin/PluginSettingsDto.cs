namespace Modules.Core.Contracts.UI.Dto.Admin
{
    using System.Runtime.Serialization;

    using Modules.Core.Contracts.UI.Dto.Data;

    [DataContract(Name = "PluginSettings")]
    public sealed class PluginSettingsDto
    {
        [DataMember]
        public TableDto Plugins { get; set; }
    }
}