namespace Infrastructure.Tests.MessageQueue
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Common.Time;
    using Infrastructure.MessageQueue;
    using Repository.Context;
    using Repository.Repositories;

    [TestFixture]
    public sealed class DatabaseQueueReaderTest
    {
        private IQueueReader _target;

        private Mock<ITimeService> _timeService;

        private Mock<IQueueRepository> _queueRepository;

        private const MessageQueueKeys Key = MessageQueueKeys.Notifications;

        private Queue _queue;

        private const string QueueMessage = "queue_message";

        private DateTime _currentDateUtc;

        [SetUp]
        public void SetUp()
        {
            _currentDateUtc = DateTime.UtcNow;

            _queue = new Queue
            {
                Body = QueueMessage,
                IsProcessed = false,
                Processed = null
            };

            _timeService = new Mock<ITimeService>();
            _timeService.Setup(_ => _.GetUtc()).Returns(_currentDateUtc);

            _queueRepository = new Mock<IQueueRepository>();

            _target = new DatabaseQueueReader(_timeService.Object, _queueRepository.Object, Key);
        }

        [Test]
        public void ShouldReturnNullWhenQueueIsEmpty()
        {
            var result = _target.Read();

            result.Should().BeNull();
        }

        [Test]
        public void ShouldTakeNextUnprocessedMessage()
        {
            _queueRepository
                .Setup(_ => _.GetNextByType(Key.ToString()))
                .Returns(new[] {_queue}.AsQueryable());

            var result = _target.Read();

            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(QueueMessage);
            _queue.IsProcessed.Should().BeTrue();
            _queue.Processed.ShouldBeEquivalentTo(_currentDateUtc);
        }
    }
}