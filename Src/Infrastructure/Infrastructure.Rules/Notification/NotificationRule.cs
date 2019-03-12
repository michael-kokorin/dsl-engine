namespace Infrastructure.Rules.Notification
{
	using System;

	using Common.Enums;
	using Infrastructure.Engines.Dsl;

	internal abstract class NotificationRule : INotificationRule
	{
		public NotificationRuleExpr Expression { get; }

		protected NotificationRule(NotificationRuleExpr expression)
		{
			Expression = expression;
		}

		public bool IsEventTriggered => Expression.Event != null;

		public bool IsTimeTriggered => Expression.Trigger != null;

		public bool IsForAllSubjects => Expression.Subjects.IsAll;

		public NotificationProtocolType DeliveryProtocol =>
			(NotificationProtocolType) Enum.Parse(typeof(NotificationProtocolType), Expression.Protocol);

		public RuleType RuleType => RuleType.Notification;
	}
}