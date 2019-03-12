namespace Modules.Core.Contracts.UI.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "User")]
	public sealed class UserDto
	{
		[DataMember(Name = "CurrentCulture")]
		public string CurrentCulture { get; set; }

		[DataMember(Name = "DisplayName")]
		public string DisplayName { get; set; }

		[DataMember(Name = "Email")]
		public string Email { get; set; }

		[DataMember(Name = "Id")]
		public long Id { get; set; }

		[DataMember(Name = "Login")]
		public string Login { get; set; }

		[DataMember(Name = "SID")]
		public string Sid { get; set; }
	}
}