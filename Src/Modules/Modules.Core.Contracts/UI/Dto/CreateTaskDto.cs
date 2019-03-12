namespace Modules.Core.Contracts.UI.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Name = "CreateTask")]
    public sealed class CreateTaskDto
    {
        [DataMember]
        public string Repository { get; set; }

        [DataMember]
        public long ProjectId { get; set; }
    }
}