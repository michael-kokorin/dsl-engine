namespace Infrastructure.Tests.MessageQueue
{
	using System;

	using Moq;

	using NUnit.Framework;

	using Common.Time;
	using Infrastructure.MessageQueue;
	using Repository.Context;
	using Repository.Repositories;

	[TestFixture]
	public sealed class DatabaseQueueWriterTest
	{
		[SetUp]
		public void SetUp()
		{
			_timeService = new Mock<ITimeService>();
			_queueRepository = new Mock<IQueueRepository>();

			_currentDateUtc = DateTime.UtcNow;

			_timeService.Setup(_ => _.GetUtc()).Returns(_currentDateUtc);

			_target = new DatabaseQueueWriter(_timeService.Object, _queueRepository.Object, Key);
		}

		private IQueueWriter _target;

		private Mock<ITimeService> _timeService;

		private Mock<IQueueRepository> _queueRepository;

		private const MessageQueueKeys Key = MessageQueueKeys.Notifications;

		private const string Message = "queue_message";

		private DateTime _currentDateUtc = DateTime.UtcNow;

		[Test]
		public void ShouldInsertNewQueueItemToDatabase()
		{
			_target.Send(Message);

			_queueRepository.Verify(_ => _.Insert(It.Is<Queue>(q =>
				q.Body.Equals(Message) &&
				q.Created.Equals(_currentDateUtc) &&
				(q.IsProcessed == false) &&
				(q.Processed == null) &&
				(q.Type == Key.ToString()))));
		}

		[Test]
		public void ShouldThrowExceptionOnEmptyMessage() => Assert.Throws<ArgumentNullException>(() => _target.Send(null));
	}
}