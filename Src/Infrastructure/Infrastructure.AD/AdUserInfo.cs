namespace Infrastructure.AD
{
	public sealed class AdUserInfo
	{
		public string DisplayName { get; set; }

		public bool IsActive { get; set; }

		public string Login { get; set; }

		public string Email { get; set; }
	}
}