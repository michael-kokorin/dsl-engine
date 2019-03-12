namespace Infrastructure.Rules.Notification
{
	using System;

	using Infrastructure.Engines.Dsl;

	internal sealed class TimeNotificationRule : NotificationRule, ITimeNotificationRule
	{
		public TimeNotificationRule(NotificationRuleExpr rule) :
				base(rule)
		{
			if (rule.Trigger == null)
				throw new ArgumentException(Resources.Resources.NotEventNotificationRuleExpression, nameof(rule));
		}

		public DateTime Start => Expression.Trigger.Start;

		public bool IsRepeatable => Expression.Trigger.Repeat.HasValue;

		public TimeSpan? Repeat => Expression.Trigger.Repeat;
	}
}