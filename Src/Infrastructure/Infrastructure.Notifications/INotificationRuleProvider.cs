namespace Infrastructure.Notifications
{
	public interface INotificationRuleProvider
	{
		long Create(long projectId,
			string name,
			string query,
			string description = null);
	}
}