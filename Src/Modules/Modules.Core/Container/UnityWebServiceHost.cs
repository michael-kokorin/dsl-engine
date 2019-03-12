namespace Modules.Core.Container
{
	using System;
	using System.ServiceModel;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	internal sealed class UnityWebServiceHost: ServiceHost
	{
		private readonly IUnityContainer _container;

		public UnityWebServiceHost([NotNull] IUnityContainer container, [NotNull] Type serviceType, params Uri[] baseAddresses)
			: base(serviceType, baseAddresses)
		{
			if(container == null) throw new ArgumentNullException(nameof(container));

			_container = container;
		}

		protected override void OnOpening()
		{
			Description.Behaviors.Add(new UnityServiceBehaviour(_container));

			base.OnOpening();
		}
	}
}