namespace Infrastructure.Notifications
{
	public interface INotificationProvider
	{
		void Publish(Notification notification);

		Notification GetNext();
	}
}