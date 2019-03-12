namespace Modules.Core.Environment
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;

	internal static class EnvironmentInitializer
	{
		public static void Initialize(
			IContainerModule[] modules,
			ReuseScope reuseScope,
			[NotNull] Action<IUnityContainer> action)
		{
			using(var container = InitializeContainer(modules, reuseScope))
			{
				action(container);
			}
		}

		[NotNull]
		public static UnityContainer InitializeContainer(
			[CanBeNull] [ItemNotNull] IReadOnlyCollection<IContainerModule> modules,
			ReuseScope reuseScope = ReuseScope.Container)
		{
			var container = new UnityContainer();

			if((modules != null) &&
				(modules.Count > 0))
			{
				container.Register(modules, reuseScope);
			}

			return container;
		}
	}
}