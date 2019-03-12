namespace Infrastructure.Rules.Notification
{
	using System;
	using System.Collections.Generic;

	using Infrastructure.Engines.Dsl;

	internal sealed class EventNotificationRule : NotificationRule, IEventNotificationRule
	{
		public EventNotificationRule(NotificationRuleExpr rule) :
			base(rule)
		{
			if (rule.Event == null)
				throw new ArgumentException(Resources.Resources.NotEventNotificationRuleExpression, nameof(rule));
		}

		public IEnumerable<string> GetDependentEvents => Expression.Event.Dependent;

		public bool HasDependentEvents => Expression.Event.HasDependent;

		public GroupActionType EventGroupAction => Expression.Event.GroupAction;

		public bool IsForAllEvents
			=> !Expression.Event.HasDependent || (Expression.Event.GroupAction == GroupActionType.Exclude);

		public bool IsEventMatch(string eventName) => Expression.Event.IsMatch(eventName);
	}
}