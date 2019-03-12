namespace Infrastructure.UserInterface
{
	using Repository.Context;

	public interface IUserInterfaceProvider
	{
		UserInterfaces GetLatest();
	}
}