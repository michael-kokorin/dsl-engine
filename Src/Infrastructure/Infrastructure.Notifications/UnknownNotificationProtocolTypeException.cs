namespace Infrastructure.Notifications
{
	using System;

	using Microsoft.Practices.Unity;

	using Common.Enums;
	using Common.Extensions;

	internal sealed class UnknownNotificationProtocolTypeException : Exception
	{
		public UnknownNotificationProtocolTypeException(NotificationProtocolType notificationProtocolType,
			// ReSharper disable once SuggestBaseTypeForParameter
			ResolutionFailedException exc) :
				base(Properties.Resources.UnknownNotificationProtocolType.FormatWith(notificationProtocolType), exc)
		{

		}
	}
}