namespace Common.Container
{
	using Microsoft.Practices.Unity;

	using Common.Extensions;

	/// <summary>
	///   Provides methods to get static access to IoC-container.
	/// </summary>
	public static class IoC
	{
		private static IUnityContainer _container;

		private static readonly object Sync = new object();

		/// <summary>
		///   Gets the container.
		/// </summary>
		/// <returns>The container.</returns>
		public static IUnityContainer GetContainer() => _container;

		private static void Init(
			IUnityContainer container,
			ReuseScope reuseScope,
			params IContainerModule[] modules)
		{
			lock(Sync)
			{
				foreach(var module in modules)
					container.Register(module, reuseScope);

				_container?.Dispose();

				_container = container;
			}
		}

		/// <summary>
		///   Initializes the default container.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <param name="modules">The modules.</param>
		public static void InitDefault(IUnityContainer builder, ReuseScope reuseScope, params IContainerModule[] modules) =>
			Init(builder, reuseScope, modules);

		/// <summary>
		///   Initializes the default container.
		/// </summary>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <param name="modules">The modules.</param>
		public static void InitDefault(ReuseScope reuseScope, params IContainerModule[] modules) =>
			InitDefault(new UnityContainer(), reuseScope, modules);

		/// <summary>
		///   Resolves instance with the specified key.
		/// </summary>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <returns>Instance of the specified type.</returns>
		public static TResult Resolve<TResult>() => _container.Resolve<TResult>();
	}
}