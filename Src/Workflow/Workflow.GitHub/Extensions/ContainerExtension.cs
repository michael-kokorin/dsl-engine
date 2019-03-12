namespace Workflow.GitHub.Extensions
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;

	internal static class ContainerExtension
	{
		public static IUnityContainer RegisterPortStage<TStage>(this IUnityContainer container, ReuseScope reuseScope) =>
			container.RegisterType<TStage>(reuseScope);
	}
}