namespace Infrastructure.Rules.Notification
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	public sealed class NotificationRuleBundle
	{
		public readonly INotificationRule NotificationRule;

		public readonly IReadOnlyDictionary<string, object> NotificationNotificationParameters;

		public NotificationRuleBundle(
			[NotNull] INotificationRule notificationRule,
			[NotNull] IDictionary<string, string> notificationParameters)
		{
			if (notificationRule == null) throw new ArgumentNullException(nameof(notificationRule));
			if (notificationParameters == null) throw new ArgumentNullException(nameof(notificationParameters));

			NotificationRule = notificationRule;

			var paramDict = new ConcurrentDictionary<string, object>();

			foreach (var parameter in notificationParameters)
			{
				paramDict.AddOrUpdate(parameter.Key, parameter.Value, (s, o) => parameter.Value);
			}

			NotificationNotificationParameters = paramDict;
		}
	}
}