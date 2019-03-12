namespace Infrastructure.Notifications
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Common.Enums;

	[UsedImplicitly]
	internal sealed class NotificationSenderProvider : INotificationSenderProvider
	{
		private readonly IUnityContainer _unityContainer;

		public NotificationSenderProvider([NotNull] IUnityContainer unityContainer)
		{
			if (unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));

			_unityContainer = unityContainer;
		}

		public INotificationSender Get(NotificationProtocolType protocolType)
		{
			try
			{
				return _unityContainer.Resolve<INotificationSender>(protocolType.ToString());
			}
			catch (ResolutionFailedException exc)
			{
				throw new UnknownNotificationProtocolTypeException(protocolType, exc);
			}
		}
	}
}