using Modules.UI;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(UnityWebActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(UnityWebActivator), "Shutdown")]

namespace Modules.UI
{
	using System.Linq;
	using System.Web.Mvc;

	using Microsoft.Practices.Unity.Mvc;

	/// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
	public static class UnityWebActivator
	{
		/// <summary>Disposes the Unity container when the application is shut down.</summary>
		// ReSharper disable once UnusedMember.Global
		public static void Shutdown()
		{
			var container = UnityConfig.GetConfiguredContainer();
			container.Dispose();
		}

		/// <summary>Integrates Unity when the application starts.</summary>
		// ReSharper disable once UnusedMember.Global
		public static void Start()
		{
			var container = UnityConfig.GetConfiguredContainer();

			FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
			FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

			DependencyResolver.SetResolver(new UnityDependencyResolver(container));

			Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(
				typeof(UnityPerRequestHttpModule));
		}
	}
}