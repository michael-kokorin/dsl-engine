namespace Infrastructure.AD
{
	using Repository.Context;

	public interface IUserProvider
	{
		Users Get(long userId);
	}
}