namespace Infrastructure.Mail
{
    using System.Xml.Serialization;

    public sealed class MailConnectionParameters
    {
        [XmlAttribute]
        public string Host { get; set; }

        [XmlAttribute]
        public string Mailbox { get; set; }

        [XmlAttribute]
        public int Port { get; set; }

        [XmlAttribute]
        public string Username { get; set; }

        [XmlAttribute]
        public bool IsSslEnabled { get; set; }

        [XmlAttribute]
        public string Password { get; set; }
    }
}