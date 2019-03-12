namespace Infrastructure.AD
{
	public interface IAdUserInfoProvider
	{
		AdUserInfo Get(string userSid);
	}
}