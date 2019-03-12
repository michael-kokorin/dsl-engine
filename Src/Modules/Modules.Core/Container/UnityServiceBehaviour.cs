namespace Modules.Core.Container
{
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Description;
	using System.ServiceModel.Dispatcher;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	internal sealed class UnityServiceBehaviour: IServiceBehavior
	{
		private readonly IUnityContainer _container;

		public UnityServiceBehaviour([NotNull] IUnityContainer container)
		{
			_container = container;
		}

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			var endpointDispatchers =
				serviceHostBase.ChannelDispatchers
											.OfType<ChannelDispatcher>()
											.SelectMany(_ => _.Endpoints);

			foreach(var endpointDispatcher in endpointDispatchers)
			{
				var endpointContainer = _container.CreateChildContainer();

				endpointDispatcher.DispatchRuntime.InstanceProvider =
					new UnityInstanceProvider(endpointContainer, serviceDescription.ServiceType);
			}
		}

		public void AddBindingParameters(
			ServiceDescription serviceDescription,
			ServiceHostBase serviceHostBase,
			Collection<ServiceEndpoint> endpoints,
			BindingParameterCollection bindingParameters)
		{
		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
		}
	}
}