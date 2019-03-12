namespace Workflow.Notification
{
	using Common.Extensions;
	using Infrastructure.Rules;
	using Infrastructure.Scheduler;
	using Repository.Repositories;

	using Quartz;

	public sealed class NotificationPollJobInitializer : ICustomJobInitializer
	{
		private readonly INotificationRuleRepository _notificationRuleRepository;

		private readonly IRuleParser _ruleParser;

		private readonly IRuleExecutorDirector _ruleExecutorDirector;

		public NotificationPollJobInitializer(INotificationRuleRepository notificationRuleRepository, IRuleParser ruleParser, IRuleExecutorDirector ruleExecutorDirector)
		{
			_notificationRuleRepository = notificationRuleRepository;
			_ruleParser = ruleParser;
			_ruleExecutorDirector = ruleExecutorDirector;
		}

		/// <summary>
		///     Initializes the specified scheduler.
		/// </summary>
		/// <param name="scheduler">The scheduler.</param>
		public void Initialize(IScheduler scheduler)
		{
			var index = 0;
			foreach(var notificationRule in _notificationRuleRepository.Query())
			{
				var parsedRule = _ruleParser.ParseNotificationRule(notificationRule.Query);
				if(!parsedRule.IsTimeTriggered)
				{
					continue;
				}

				var job = new NotificationPollJob(_notificationRuleRepository, _ruleParser, _ruleExecutorDirector)
				{
					Interval = parsedRule.Expression.Trigger.Repeat.Value
				};

				var scheduledJob = JobBuilder
					.Create(job.GetType())
					.WithIdentity("NotificationPollJob.Rule_{0}".FormatWith(notificationRule.Id))
					.UsingJobData("NotificationRuleId", notificationRule.Id)
					.Build();

				var trigger = TriggerBuilder
					.Create()
					.WithIdentity("NotificationPollTrigger.Rule_{0}".FormatWith(notificationRule.Id))
					.StartAt(SystemTime.UtcNow().AddSeconds(20 * index))
					.WithSimpleSchedule(x => x.WithInterval(job.Interval).RepeatForever())
					.Build();

				scheduler.ScheduleJob(scheduledJob, trigger);
				index++;
			}
		}
	}
}