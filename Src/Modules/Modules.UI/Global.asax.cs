namespace Modules.UI
{
	using System.Reflection;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Optimization;
	using System.Web.Routing;

	using Common.Container;
	using Modules.Core.Contracts.UI;
	using Modules.Core.Contracts.UI.Dto;
	using Modules.UI.Extensions;

	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		protected void Application_BeginRequest() => FirstTimeAppInitializer.Initialize();

		private static class FirstTimeAppInitializer
		{
			private static bool _isInitialized;

			private static readonly object Locker = new object();

			public static void Initialize()
			{
				lock (Locker)
				{
					if (_isInitialized) return;

					RegisterModule();

					_isInitialized = true;
				}
			}
		}

		private static void RegisterModule()
		{
			var service = IoC.Resolve<IApiService>();

			service.CheckVersion(new UserInterfaceInfoDto
			{
				Host = HttpContext.Current.Request.GetBaseUrl(),
				Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
			});
		}
	}
}