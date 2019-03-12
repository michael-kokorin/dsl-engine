namespace Modules.Core.Contracts.UI.Dto.Admin
{
    using System.Runtime.Serialization;

    [DataContract(Name = "Settings")]
    public sealed class SettingsDto
    {
        [DataMember]
        public ActiveDirectorySettingsDto ActiveDirectorySettings { get; set; }

        [DataMember]
        public DatabaseSettingsDto DatabaseSettings { get; set; }

        [DataMember]
        public FileStorageSettingsDto FileStorageSettings { get; set; }

        [DataMember]
        public ScanAgentSettingsDto ScanAgentSettings { get; set; }

        [DataMember]
        public PluginSettingsDto PluginSettings { get; set; }

        [DataMember]
        public NotificationSettingsDto NotificationSettings { get; set; }
    }
}