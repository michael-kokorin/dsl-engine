namespace Infrastructure.RequestHandling
{
	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	[UsedImplicitly]
	internal sealed class RequestExecutorProvider : IRequestExecutorProvider
	{
		private readonly IUnityContainer _container;

		public RequestExecutorProvider(IUnityContainer container)
		{
			_container = container;
		}

		/// <summary>
		///   Gets executor for the specified source type and request method.
		/// </summary>
		/// <param name="sourceType">Type of the source.</param>
		/// <returns>The request executor.</returns>
		public IRequestExecutor Get(string sourceType) => _container.Resolve<IRequestExecutor>(sourceType);
	}
}