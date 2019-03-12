namespace Infrastructure.Notifications
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Logging;
	using Common.Time;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class NotificationRuleProvider : INotificationRuleProvider
	{
		private readonly ILog _log;

		private readonly INotificationRuleRepository _notificationRuleRepository;

		private readonly ITimeService _timeService;

		public NotificationRuleProvider(
			[NotNull] ILog log,
			[NotNull] INotificationRuleRepository notificationRuleRepository,
			[NotNull] ITimeService timeService)
		{
			if (log == null) throw new ArgumentNullException(nameof(log));
			if (notificationRuleRepository == null) throw new ArgumentNullException(nameof(notificationRuleRepository));
			if (timeService == null) throw new ArgumentNullException(nameof(timeService));

			_log = log;
			_notificationRuleRepository = notificationRuleRepository;
			_timeService = timeService;
		}

		public long Create(long projectId,
			string name,
			string query,
			string description = null)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException(nameof(query));

			if (_notificationRuleRepository.Get(projectId, name).Any())
				throw new NotificationAlreadyExistsException(projectId, name);

			var rule = new NotificationRules
			{
				ProjectId = projectId,
				Added = _timeService.GetUtc(),
				Description = description,
				DisplayName = name,
				Query = query
			};

			_notificationRuleRepository.Insert(rule);

			_notificationRuleRepository.Save();

			_log.Info(Properties.Resources.NotificationHasBeenCreated.FormatWith(rule.Id, name));

			return rule.Id;
		}
	}
}