namespace Common.Container
{
	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Common.Extensions;

	[UsedImplicitly]
	internal sealed class Container : IContainer
	{
		private readonly IUnityContainer _unityContainer;

		public Container() : this(new UnityContainer())
		{

		}

		internal Container(IUnityContainer container)
		{
			_unityContainer = container;

			RegisterInstance<IContainer>(this, ReuseScope.Container);
		}

		public IContainer CreateChild() => new Container(_unityContainer.CreateChildContainer());

		public IContainer RegisterInstance<TAs>(TAs instance, ReuseScope reuseScope)
		{
			_unityContainer.RegisterInstance<TAs>(instance, reuseScope);

			return this;
		}

		public IContainer RegisterType<TAs, TOf>(ReuseScope reuseScope) where TOf : TAs
		{
			_unityContainer.RegisterType<TAs, TOf>(reuseScope);

			return this;
		}

		public IContainer RegisterNamed<TAs, TOf>(string name, ReuseScope reuseScope) where TOf : TAs
		{
			_unityContainer.RegisterType<TAs, TOf>(name, reuseScope);

			return this;
		}

		TAs IContainer.Resolve<TAs>() => _unityContainer.Resolve<TAs>();

		TAs IContainer.ResolveNamed<TAs>(string name) => _unityContainer.Resolve<TAs>(name);
	}
}