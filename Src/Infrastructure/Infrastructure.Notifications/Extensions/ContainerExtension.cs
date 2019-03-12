namespace Infrastructure.Notifications.Extensions
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Enums;
	using Common.Extensions;
	using Common.Validation;
	using Infrastructure.Notifications;

	internal static class ContainerExtension
	{
		public static IUnityContainer RegisterNotificationSender<T>(this IUnityContainer container,
			NotificationProtocolType protocolType,
			ReuseScope reuseScope)
			where T : class, INotificationSender
			=> container.RegisterType<INotificationSender, T>(protocolType.ToString(), reuseScope);

		public static IUnityContainer RegisterValidator<TData, TValidator>(this IUnityContainer container,
			ReuseScope reuseScope)
			where TValidator : IValidator<TData> => container.RegisterType<IValidator<TData>, TValidator>(reuseScope);
	}
}