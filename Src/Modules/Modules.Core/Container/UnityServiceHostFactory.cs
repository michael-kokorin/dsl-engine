namespace Modules.Core.Container
{
	using System;
	using System.Configuration;
	using System.ServiceModel;
	using System.ServiceModel.Activation;

	using Common.Container;

	public sealed class UnityServiceHostFactory: ServiceHostFactory
	{
		protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			var container = IoC.GetContainer();

			if(container == null)
				throw new ConfigurationErrorsException();

			var childContainer = container.CreateChildContainer();

			return new UnityWebServiceHost(childContainer, serviceType, baseAddresses);
		}
	}
}