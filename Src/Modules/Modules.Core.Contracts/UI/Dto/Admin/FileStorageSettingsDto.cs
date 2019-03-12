namespace Modules.Core.Contracts.UI.Dto.Admin
{
    using System.Runtime.Serialization;

    [DataContract(Name = "FileStorageSettings")]
    public sealed class FileStorageSettingsDto
    {
        [DataMember]
        public string TempDirPath { get; set; }
    }
}