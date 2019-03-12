namespace Modules.Core.Container
{
	using System;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Dispatcher;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	internal sealed class UnityInstanceProvider: IInstanceProvider
	{
		private readonly IUnityContainer _container;

		private readonly Type _serviceType;

		public UnityInstanceProvider(
			[NotNull] IUnityContainer container,
			[NotNull] Type serviceType)
		{
			if(container == null) throw new ArgumentNullException(nameof(container));
			if(serviceType == null) throw new ArgumentNullException(nameof(serviceType));

			_serviceType = serviceType;
			_container = container;
		}

		public object GetInstance(InstanceContext instanceContext) => Resolve();

		public object GetInstance(InstanceContext instanceContext, Message message) => Resolve();

		public void ReleaseInstance(InstanceContext instanceContext, object instance)
		{
		}

		private object Resolve() => _container
			.CreateChildContainer()
			.Resolve(_serviceType);
	}
}