namespace Workflow.Event.Handlers
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Logging;
	using Infrastructure;
	using Infrastructure.Events;
	using Infrastructure.Rules;
	using Infrastructure.Rules.Notification;
	using Repository.Context;
	using Repository.Repositories;
	using Workflow.Properties;

	[UsedImplicitly]
	internal sealed class NotificationEventHandler : IEventHandler
	{
		private readonly INotificationRuleRepository _notificationRuleRepository;

		private readonly IRuleExecutorDirector _ruleExecutorDirector;

		private readonly ILog _log;

		private readonly IRuleParser _ruleParser;

		public NotificationEventHandler(
			[NotNull] INotificationRuleRepository notificationRuleRepository,
			[NotNull] IRuleParser ruleParser,
			[NotNull] IRuleExecutorDirector ruleExecutorDirector,
			[NotNull] ILog log)
		{
			if (notificationRuleRepository == null) throw new ArgumentNullException(nameof(notificationRuleRepository));
			if (ruleParser == null) throw new ArgumentNullException(nameof(ruleParser));
			if (ruleExecutorDirector == null) throw new ArgumentNullException(nameof(ruleExecutorDirector));
			if (log == null) throw new ArgumentNullException(nameof(log));

			_notificationRuleRepository = notificationRuleRepository;
			_ruleParser = ruleParser;
			_ruleExecutorDirector = ruleExecutorDirector;
			_log = log;
		}

		/// <summary>
		///   Determines whether this instance can handle the specified event to handle.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		/// <returns><see langword="true" /> if this instance can handle the event; otherwise, <see langword="false" />.</returns>
		public bool CanHandle(Event eventToHandle) => true;

		/// <summary>
		///   Handles the specified event to handle.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		public void Handle(Event eventToHandle)
		{
			try
			{
				var data = eventToHandle.Data;

				var projectId = data.ContainsKey(Variables.ProjectId)
					? long.Parse(data[Variables.ProjectId])
					: default(long?);

				var projectNotifications = _notificationRuleRepository.GetByProject(projectId);

				foreach (var notificationRule in projectNotifications)
				{
					ProcessEventNotification(eventToHandle, notificationRule, data);
				}
			}
			catch (Exception exc)
			{
				_log.Error(Resources.EventProcessingError.FormatWith(eventToHandle.Key), exc);
			}
		}

		private void ProcessEventNotification(Event eventToHandle, NotificationRules notificationRule, IDictionary<string, string> data)
		{
			try
			{
				var parcedRule = _ruleParser.ParseNotificationRule(notificationRule.Query);

				var eventRule = parcedRule as IEventNotificationRule;

				if (eventRule == null)
				{
					_log.Warning("Event notification rule has incorrect type. Type='{0}'".FormatWith(parcedRule.GetType().FullName));

					return;
				}

				if (!(eventRule.IsForAllEvents || (eventRule.HasDependentEvents && eventRule.IsEventMatch(eventToHandle.Key))))
				{
					_log.Trace("Event notification rule skipped. Current event='{0}', Target event='{0}'".FormatWith(
						eventToHandle.Key,
						eventRule.GetDependentEvents.ToCommaSeparatedString()));

					return;
				}

				_ruleExecutorDirector.Execute<INotificationRule, NotificationRuleResult>((dynamic) eventRule, data);

				_log.Info("Event notification rule processed. Event='{0}', Notification rule Id='{1}', Notification rule name='{2}'"
					.FormatWith(
						eventToHandle.Key,
						notificationRule.Id,
						notificationRule.DisplayName));
			}
			catch (Exception exc)
			{
				_log.Error(
					"Event notification rule processing has failed. Event='{0}', Notification rule Id='{1}', Notification rule name='{2}'"
						.FormatWith(eventToHandle.Key, notificationRule.Id, notificationRule.DisplayName),
					exc);
			}
		}
	}
}