namespace Workflow.Notification
{
	using System.Collections.Generic;

	using Infrastructure;
	using Infrastructure.Rules;
	using Infrastructure.Rules.Notification;
	using Infrastructure.Scheduler;
	using Repository.Repositories;

	using Quartz;

	/// <summary>
	///     Performs operations to start notifications with time triggers.
	/// </summary>
	/// <seealso cref="Infrastructure.Scheduler.ScheduledJob"/>
	/// <seealso cref="Infrastructure.Scheduler.ICustomScheduledJob"/>
	[DisallowConcurrentExecution]
	public sealed class NotificationPollJob : ScheduledJob, ICustomScheduledJob
	{
		private readonly INotificationRuleRepository _notificationRuleRepository;

		private readonly IRuleParser _ruleParser;

		private readonly IRuleExecutorDirector _ruleExecutorDirector;

		public NotificationPollJob(
			INotificationRuleRepository notificationRuleRepository,
			IRuleParser ruleParser,
			IRuleExecutorDirector ruleExecutorDirector)
		{
			_notificationRuleRepository = notificationRuleRepository;
			_ruleParser = ruleParser;
			_ruleExecutorDirector = ruleExecutorDirector;
		}

		/// <summary>
		///     Executes the job.
		/// </summary>
		/// <returns>
		///     Positive value to repeat the task, negative value or 0 - to finish the task and to wait time interval before the
		///     next run.
		/// </returns>
		protected override int Process()
		{
			var ruleId = (long) JobExecutionContext.MergedJobDataMap.Get("NotificationRuleId");
			var rule = _notificationRuleRepository.GetById(ruleId);

			var parsedRule = _ruleParser.ParseNotificationRule(rule.Query);
			_ruleExecutorDirector.Execute<INotificationRule, NotificationRuleResult>(
				parsedRule,
				new Dictionary<string, string>
				{
					{ Variables.ProjectId, rule.ProjectId.ToString() }
				});

			return 0;
		}
	}
}