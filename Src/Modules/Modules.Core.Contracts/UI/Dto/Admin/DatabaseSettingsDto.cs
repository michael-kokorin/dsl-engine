namespace Modules.Core.Contracts.UI.Dto.Admin
{
    using System.Runtime.Serialization;

    [DataContract(Name = "DatabaseSettings")]
    public sealed class DatabaseSettingsDto
    {
        [DataMember]
        public string ConnectionString { get; set; }
    }
}