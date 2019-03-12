namespace Modules.Core.Contracts.UI.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "NotificationRule")]
	public sealed class NotificationRuleDto
	{
		[DataMember]
		public string DisplayName { get; set; }

		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public string Query { get; set; }
	}
}