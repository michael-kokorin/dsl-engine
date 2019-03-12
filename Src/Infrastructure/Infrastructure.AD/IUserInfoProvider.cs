namespace Infrastructure.AD
{
	using Common.Security;

	public interface IUserInfoProvider
	{
		UserInfo Create(UserInfo userInfo);

		UserInfo Get(string userSid);

		void Update(long userId, string login, string displayName, string emal);
	}
}