namespace Infrastructure.Notifications
{
	using Common.Enums;

	public interface INotificationSenderProvider
	{
		INotificationSender Get(NotificationProtocolType protocolType);
	}
}