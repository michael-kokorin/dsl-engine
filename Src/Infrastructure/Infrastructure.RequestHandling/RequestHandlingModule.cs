namespace Infrastructure.RequestHandling
{
	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;

	/// <summary>
	/// Represents container module for the project.
	/// </summary>
	/// <seealso cref="Common.Container.IContainerModule" />
	[UsedImplicitly]
	public sealed class RequestHandlingModule : IContainerModule
	{
		/// <summary>
		///   Registers binding to the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) =>
			container
				.RegisterType<IRequestExecutorProvider, RequestExecutorProvider>(reuseScope)
				.RegisterType<IRequestExecutor, ScanAgentRequestExecutor>("SA", reuseScope)
				.RegisterType<ISAParameterTranslatorProvider, SAParameterTranslatorProvider>(reuseScope)
				.RegisterType<ISAParameterTranslator, VulnerabilitySearchTimeoutSAParameterTranslator>(
					typeof(VulnerabilitySearchTimeoutSAParameterTranslator).FullName,
					reuseScope)
				.RegisterType<ISAParameterTranslator, SiteAddressSAParameterTranslator>(
					typeof(SiteAddressSAParameterTranslator).FullName,
					reuseScope)
				.RegisterType<ISAParameterTranslator, LaunchParameterTranslator>(
					typeof(LaunchParameterTranslator).FullName,
					reuseScope)
				.RegisterType<ISAParameterTranslator, SolutionFileParameterTranslator>(
					typeof(SolutionFileParameterTranslator).FullName,
					reuseScope)
				.RegisterType<ISAParameterTranslator, RootScanFolderParameterTranslator>(
					typeof(RootScanFolderParameterTranslator).FullName,
					reuseScope);
	}
}