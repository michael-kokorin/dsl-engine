namespace Modules.Core.Contracts.UI.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Name = "UserUpdated")]
    public sealed class UserUpdatedDto
    {
        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Email { get; set; }
    }
}