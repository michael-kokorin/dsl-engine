namespace Common.Container
{
	using Microsoft.Practices.Unity;

	/// <summary>
	///   Provides method to build lifetime manager.
	/// </summary>
	internal interface ILifeManagerFactory
	{
		/// <summary>
		///   Builds the lifetime manager using specified reuse scope.
		/// </summary>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>Lifetime manager.</returns>
		LifetimeManager Build(ReuseScope reuseScope);
	}
}