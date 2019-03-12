namespace Modules.UI
{
	using System;

	using Microsoft.Practices.Unity;

	using Common;
	using Common.Container;

	internal static class UnityConfig
	{
		private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
		{
			var container = new UnityContainer();

			RegisterTypes(container);

			return container;
		});

		public static IUnityContainer GetConfiguredContainer() => Container.Value;

		private static void RegisterTypes(IUnityContainer container) =>
				IoC.InitDefault(
						container,
						ReuseScope.PerRequest,
						new CommonContainerModule(),
						new UiContainerModule());
	}
}