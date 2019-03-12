namespace Modules.Core.Contracts.UI.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Name = "AuthorityRequest")]
    public sealed class AuthorityRequestDto
    {
        [DataMember(IsRequired = true)]
        public string[] AuthorityNames { get; set; }

        [DataMember(IsRequired = false)]
        public long? EntityId { get; set; }
    }
}