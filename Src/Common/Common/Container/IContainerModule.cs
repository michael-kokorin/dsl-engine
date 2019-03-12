namespace Common.Container
{
	using Microsoft.Practices.Unity;

	/// <summary>
	///   Represents contract for the IoC container registration module.
	/// </summary>
	public interface IContainerModule
	{
		/// <summary>
		///   Registers binding to the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope);
	}
}