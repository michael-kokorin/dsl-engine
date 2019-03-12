namespace Modules.SA
{
	using System.ServiceModel;

	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Common.Transaction;
	using Infrastructure.Plugins.Common;
	using Infrastructure.Plugins.Agent.Client.Contracts;
	using Infrastructure.Scheduler;
	using Modules.Core.Contracts.ExternalSystems;
	using Modules.SA.Config;

	internal sealed class ScanAgentContainerModule: IContainerModule
	{
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) => container
			.RegisterType<ScheduledJob, ScanAgent>(typeof(ScanAgent).FullName, reuseScope)
			.RegisterType<IUnitOfWork, EmptyUnitOfWork>(reuseScope)
			.RegisterType<IPluginLoader<IAgentClientPlugin>, PluginLoader<IAgentClientPlugin>>(reuseScope)
			.RegisterType<IPluginProvider, PluginProvider>(reuseScope)
			.RegisterType<IScanAgentIdGenerator, ScanAgentIdGenerator>(reuseScope)
			.RegisterType<IScanAgentIdProvider, FileScanAgentIdProvider>(reuseScope)
			.RegisterType<IScanAgentIdFilePathProvider, ScanAgentIdFilePathProvider>(reuseScope)
			.RegisterType<IApiService>(
				reuseScope,
				new InjectionFactory(
					x =>
					{
						var factory = new ChannelFactory<IApiService>("ApiService");
						return factory.CreateChannel();
					}));
	}
}