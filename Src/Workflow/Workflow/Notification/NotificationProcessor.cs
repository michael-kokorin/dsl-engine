namespace Workflow.Notification
{
    using JetBrains.Annotations;

    using Infrastructure.Notifications;

    [UsedImplicitly]
    internal sealed class NotificationProcessor : INotificationProcessor
    {
        private readonly INotificationProvider _notificationProvider;

        private readonly INotificationSendDirector _notificationSendDirector;

        public NotificationProcessor(
            [NotNull] INotificationProvider notificationProvider,
            [NotNull] INotificationSendDirector notificationSendDirector)
        {
            _notificationProvider = notificationProvider;
            _notificationSendDirector = notificationSendDirector;
        }

        public bool ProcessNext()
        {
            var notificationToProcess = _notificationProvider.GetNext();

            if(notificationToProcess == null)
                return false;

            _notificationSendDirector.Send(notificationToProcess);

            return true;
        }
    }
}