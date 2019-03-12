namespace Modules.Core.Contracts.UI.Dto.Admin
{
    using System.Runtime.Serialization;

    [DataContract(Name = "ActiveDirectorySettings")]
    public sealed class ActiveDirectorySettingsDto
    {
        [DataMember]
        public string RootGroupPath { get; set; }
    }
}