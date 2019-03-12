namespace Infrastructure.Notifications
{
    public interface INotificationSendDirector
    {
        void Send(Notification notification);
    }
}