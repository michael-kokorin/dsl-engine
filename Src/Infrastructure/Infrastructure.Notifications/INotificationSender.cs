namespace Infrastructure.Notifications
{
	public interface INotificationSender
	{
		void Send(Notification notification);
	}
}