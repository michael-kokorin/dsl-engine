namespace Infrastructure.Reports.Generation.Stages
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	[UsedImplicitly]
	public sealed class ReportGenerationStageDispatcher : IReportGenerationStageDispatcher
	{
		private readonly IUnityContainer _unityContainer;

		public ReportGenerationStageDispatcher([NotNull] IUnityContainer unityContainer)
		{
			if (unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));

			_unityContainer = unityContainer;
		}

		public IReportGenerationStage Get<T>()
			where T : IReportGenerationStage
		{
			try
			{
				return _unityContainer.Resolve<T>();
			}
			catch (ResolutionFailedException ex)
			{
				throw new UnknownReportGenerationStageException(typeof(T), ex);
			}
		}
	}
}