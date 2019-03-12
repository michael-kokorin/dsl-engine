namespace Infrastructure.MessageQueue
{
	using System;

	using JetBrains.Annotations;

	using Common.Logging;
	using Common.Time;
	using Repository.Context;
	using Repository.Repositories;

	internal sealed class DatabaseQueueWriter : IQueueWriter
	{
		private readonly ITimeService _timeService;

		private readonly IQueueRepository _queueRepository;

		private readonly MessageQueueKeys _key;

		public DatabaseQueueWriter(
			[NotNull] ITimeService timeService,
			[NotNull] IQueueRepository queueRepository,
			MessageQueueKeys key)
		{
			if (timeService == null) throw new ArgumentNullException(nameof(timeService));
			if (queueRepository == null) throw new ArgumentNullException(nameof(queueRepository));

			_timeService = timeService;
			_queueRepository = queueRepository;
			_key = key;
		}

		public void Dispose()
		{

		}

		[LogMethod]
		public void Send(string message)
		{
			if (string.IsNullOrEmpty(message))
				throw new ArgumentNullException(nameof(message));

			var queueElement = new Queue
			{
				Body = message,
				Created = _timeService.GetUtc(),
				IsProcessed = false,
				Processed = null,
				Type = _key.ToString()
			};

			_queueRepository.Insert(queueElement);
		}
	}
}