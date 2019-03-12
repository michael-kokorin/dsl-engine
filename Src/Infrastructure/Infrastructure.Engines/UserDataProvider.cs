namespace Infrastructure.Engines
{
	using System;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Extensions;
	using Repository.Context;

	[UsedImplicitly]
	internal sealed class UserDataProvider : IUserDataProvider
	{
		public string GetDeliveryContacts(NotificationProtocolType protocol, Users user)
		{
			switch (protocol)
			{
				case NotificationProtocolType.Email:
					return user.Email;
				default:
					throw new Exception(Resources.Resources.NotificationProtocolIsNotSupported.FormatWith(protocol));
			}
		}
	}
}