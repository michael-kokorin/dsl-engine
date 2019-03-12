namespace Infrastructure.Notifications
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Enums;
	using Common.Extensions;
	using Infrastructure.Notifications.Extensions;

	public sealed class NotificationsContainerModule : IContainerModule
	{
		/// <summary>
		///   Registers binding to the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) =>
			container
				.RegisterType<INotificationProvider, NotificationProvider>(reuseScope)
				.RegisterType<INotificationRuleProvider, NotificationRuleProvider>(reuseScope)
				.RegisterType<INotificationSendDirector, NotificationSendDirector>(reuseScope)
				.RegisterType<INotificationSenderProvider, NotificationSenderProvider>(reuseScope)

				.RegisterNotificationSender<MailNotificationSender>(NotificationProtocolType.Email, reuseScope)

				.RegisterValidator<Notification, NotificationValidator>(reuseScope);
	}
}