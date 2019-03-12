namespace Modules.Core.Contracts.UI.Dto.Admin
{
    using System.Runtime.Serialization;

    [DataContract(Name = "NotificationSettings")]
    public sealed class NotificationSettingsDto
    {
        [DataMember]
        public string MailServerHost { get; set; }

        [DataMember]
        public int MainServerPort { get; set; }

        [DataMember]
        public string MailBox { get; set; }

        [DataMember]
        public bool IsSslEnabled { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}