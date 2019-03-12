namespace Workflow.GitHub
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Workflow.GitHub.Extensions;
	using Workflow.GitHub.Stages.PostProcessing;

	/// <summary>
	///   Represents container module for the project.
	/// </summary>
	/// <seealso cref="Common.Container.IContainerModule"/>
	public sealed class GitHubWorkflowContainerModule : IContainerModule
	{
		/// <summary>
		///   Registers binding to the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) =>
			container
				.RegisterType<IAnnotationIssuesProvider, IssueAnnotationsProvider>(reuseScope)
				.RegisterType<IVulnerabilitiesProcessor, VulnerabilitiesProcessor>(reuseScope)
				.RegisterType<IIssueNameBuilder, IssueNameBuilder>(reuseScope)
				.RegisterType<IIssueAnnotationStateSerializer, IssueAnnotationStateSerializer>(reuseScope)
				.RegisterType<IIssueAnnotationWorkflow, IssueAnnotationWorkflow>(reuseScope)
				.RegisterType<IIssueAnnotationFormatter, SharpSingleLineIssueAnnotationFormatter>(reuseScope)
				.RegisterType<IIssueAnnotationSerializer, IssueAnnotationSerializer>(reuseScope)
				.RegisterType<IBackendPluginProvider, BackendPluginProvider>(reuseScope)
				.RegisterType<IPostProcessingStageProvider, PostProcessingStageProvider>()
				.RegisterType<IVulnerabilityInfoProvider, VulnerabilityInfoProvider>()

				.RegisterPortStage<FilterByBranchPostProcessingStage>(reuseScope)
				.RegisterPortStage<GetRemainingIssuesPostProcessingStage>(reuseScope)
				.RegisterPortStage<LoadIssueAnnotationsPostProcessingStage>(reuseScope)
				.RegisterPortStage<LoadIssuesPostProcessingStage>(reuseScope)
				.RegisterPortStage<LoadItPluginPostProcessingStage>(reuseScope)
				.RegisterPortStage<LoadVcsPluginPostProcessingStage>(reuseScope)
				.RegisterPortStage<LoadVulnInfoPostProcessingStage>(reuseScope)
				.RegisterPortStage<ProcessRemainingIssuesPostProcessingStage>(reuseScope)
				.RegisterPortStage<SaveToItPostProcessingStage>(reuseScope)
				.RegisterPortStage<SaveToVcsPostProcessingStage>(reuseScope)
				.RegisterPortStage<UpdatePairsPostProcessingStage>(reuseScope)
				.RegisterPortStage<UpdateVulnerabilitiesPostProcessingStage>(reuseScope);
	}
}