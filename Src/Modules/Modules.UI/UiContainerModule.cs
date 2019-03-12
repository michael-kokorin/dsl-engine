namespace Modules.UI
{
	using System.ServiceModel;

	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Modules.Core.Contracts.Query;
	using Modules.Core.Contracts.Report;
	using Modules.Core.Contracts.UI;
	using Modules.UI.NodeProviders;
	using Modules.UI.Services;

	internal sealed class UiContainerModule : IContainerModule
	{
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) => container
			.RegisterType<IAuthorityProvider, ApiAuthorityProvider>(reuseScope)
			.RegisterType<ProjectNodeProvider>(reuseScope)

			.RegisterType<IApiService>(reuseScope,
				new InjectionFactory(x => CreateProxy<IApiService>("ApiService")))
			.RegisterType<IQueryService>(reuseScope,
				new InjectionFactory(x => CreateProxy<IQueryService>("QueryService")))
			.RegisterType<IReportService>(reuseScope,
				new InjectionFactory(x => CreateProxy<IReportService>("ReportService")));

		private static T CreateProxy<T>(string serviceName)
		{
			var factory = new ChannelFactory<T>(serviceName);

			var proxy = factory.CreateChannel();

			return proxy;
		}
	}
}