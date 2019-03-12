namespace Modules.Core.Contracts.UI.Dto.UserSettings
{
    using System.Runtime.Serialization;

    [DataContract(Name = "Plugin")]
    public sealed class PluginDto
    {
        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public PluginTypeDto Type { get; set; }
    }

    [DataContract(Name = "PluginType")]
    public enum PluginTypeDto
    {
        [EnumMember] IssueTracker = 0,

        [EnumMember] VersionControl = 1
    }
}