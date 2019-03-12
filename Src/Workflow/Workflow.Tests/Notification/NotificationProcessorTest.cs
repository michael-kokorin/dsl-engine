namespace Workflow.Tests.Notification
{
    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Infrastructure.Notifications;
    using Workflow.Notification;

    [TestFixture]
    public sealed class NotificationProcessorTest
    {
        private INotificationProcessor _target;

        private Mock<INotificationProvider> _notificationProvider;

        private Mock<INotificationSendDirector> _notificationSendDirector;

        [SetUp]
        public void SetUp()
        {
            _notificationProvider = new Mock<INotificationProvider>();

            _notificationSendDirector = new Mock<INotificationSendDirector>();

            _target = new NotificationProcessor(_notificationProvider.Object, _notificationSendDirector.Object);
        }

        [Test]
        public void ShouldReturnFalseWhenNotificationNotFound()
        {
            _notificationProvider.Setup(_ => _.GetNext()).Returns((Notification) null);

            var result = _target.ProcessNext();

            result.Should().BeFalse();

            _notificationProvider.Verify(_ => _.GetNext(), Times.Once);
        }

        [Test]
        public void ShouldSendNotification()
        {
            var notification = new Notification();

            _notificationProvider.Setup(_ => _.GetNext()).Returns(notification);

            var result = _target.ProcessNext();

            result.Should().BeTrue();

            _notificationProvider.Verify(_ => _.GetNext(), Times.Once);
            _notificationSendDirector.Verify(_ => _.Send(notification), Times.Once);
        }
    }
}