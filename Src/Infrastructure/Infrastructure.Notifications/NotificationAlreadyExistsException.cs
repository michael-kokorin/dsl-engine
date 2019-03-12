namespace Infrastructure.Notifications
{
	using System;

	using Common.Extensions;

	internal sealed class NotificationAlreadyExistsException : Exception
	{
		public NotificationAlreadyExistsException(long? projectId, string notificationName)
			: base(Properties.Resources.NotificationAlreadyExists.FormatWith(projectId, notificationName))
		{

		}
	}
}