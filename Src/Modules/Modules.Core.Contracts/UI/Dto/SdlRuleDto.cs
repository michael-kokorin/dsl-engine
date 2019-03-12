namespace Modules.Core.Contracts.UI.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Name = "SdlRule")]
    public sealed class SdlRuleDto
    {
        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Query { get; set; }
    }
}