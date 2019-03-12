namespace Infrastructure.MessageQueue
{
    using JetBrains.Annotations;

    using Common.Logging;
    using Common.Time;
    using Repository.Repositories;

    internal sealed class DatabaseMessageQueue : IMessageQueue
    {
        private readonly IQueueRepository _queueRepository;

        private readonly ITimeService _timeService;

        public DatabaseMessageQueue(
            [NotNull] ITimeService timeService,
            [NotNull] IQueueRepository queueRepository)
        {
            _timeService = timeService;
            _queueRepository = queueRepository;
        }

        [LogMethod(LogInputParameters = true)]
        public IQueueReader BeginRead(MessageQueueKeys key) =>
            new DatabaseQueueReader(_timeService, _queueRepository, key);

        [LogMethod(LogInputParameters = true)]
        public IQueueWriter BeginWrite(MessageQueueKeys key) =>
            new DatabaseQueueWriter(_timeService, _queueRepository, key);
    }
}