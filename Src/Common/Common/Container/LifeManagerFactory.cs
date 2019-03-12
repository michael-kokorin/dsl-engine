namespace Common.Container
{
	using System.ComponentModel;

	using Microsoft.Practices.Unity;

	internal sealed class LifeManagerFactory: ILifeManagerFactory
	{
		/// <summary>
		///   Builds the lifetime manager using specified reuse scope.
		/// </summary>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>Lifetime manager.</returns>
		public LifetimeManager Build(ReuseScope reuseScope)
		{
			switch(reuseScope)
			{
				case ReuseScope.Container:
					return new ContainerControlledLifetimeManager();
				case ReuseScope.Hierarchy:
					return new HierarchicalLifetimeManager();
				case ReuseScope.PerRequest:
					return new PerRequestLifetimeManager();
				case ReuseScope.PerResolve:
					return new PerResolveLifetimeManager();
				case ReuseScope.PerThread:
					return new PerThreadLifetimeManager();
				case ReuseScope.External:
					return new ExternallyControlledLifetimeManager();
				default:
					throw new InvalidEnumArgumentException(nameof(reuseScope), (int)reuseScope, reuseScope.GetType());
			}
		}
	}
}