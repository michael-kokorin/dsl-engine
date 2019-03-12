namespace Modules.Core.Contracts.UI.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Name = "Authority")]
    public sealed class AuthorityDto
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}