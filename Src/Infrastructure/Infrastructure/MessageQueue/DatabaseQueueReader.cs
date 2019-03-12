namespace Infrastructure.MessageQueue
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Time;
	using Repository.Repositories;

	internal sealed class DatabaseQueueReader : IQueueReader
	{
		private readonly ITimeService _timeService;

		private readonly IQueueRepository _queueRepository;

		private readonly MessageQueueKeys _key;

		public DatabaseQueueReader(
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

		public string Read()
		{
			var queueElement = _queueRepository
				.GetNextByType(_key.ToString())
				.FirstOrDefault();

			if (queueElement == null)
				return null;

			queueElement.IsProcessed = true;
			queueElement.Processed = _timeService.GetUtc();

			return queueElement.Body;
		}
	}
}