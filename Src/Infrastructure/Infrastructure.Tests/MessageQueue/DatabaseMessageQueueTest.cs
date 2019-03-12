namespace Infrastructure.Tests.MessageQueue
{
    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Common.Time;
    using Infrastructure.MessageQueue;
    using Repository.Repositories;

    [TestFixture]
    public sealed class DatabaseMessageQueueTest
    {
        private IMessageQueue _target;

        private Mock<ITimeService> _timeService;

        private Mock<IQueueRepository> _queueRepository;

        private MessageQueueKeys _key;

        [SetUp]
        public void SetUp()
        {
            _timeService = new Mock<ITimeService>();
            _queueRepository = new Mock<IQueueRepository>();

            _target = new DatabaseMessageQueue(_timeService.Object, _queueRepository.Object);

            _key = MessageQueueKeys.Notifications;
        }

        [Test]
        public void ShouldReturnDatabaseQueueReader()
        {
            var result = _target.BeginRead(_key);

            result.Should().NotBeNull();
            result.Should().BeOfType<DatabaseQueueReader>();
        }

        [Test]
        public void ShouldReturnDatabaseQueueWriter()
        {
            var result = _target.BeginWrite(_key);

            result.Should().NotBeNull();
            result.Should().BeOfType<DatabaseQueueWriter>();
        }
    }
}