namespace Infrastructure.Reports.Blocks
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	[UsedImplicitly]
	internal sealed class ReportBlockVizualizerFabric : IReportBlockVizualizerFabric
	{
		private readonly IUnityContainer _unityContainer;

		public ReportBlockVizualizerFabric([NotNull] IUnityContainer unityContainer)
		{
			if (unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));

			_unityContainer = unityContainer;
		}

		public IReportBlockVizualizer<T> Get<T>([NotNull] T block) where T : class, IReportBlock
		{
			if (block == null) throw new ArgumentNullException(nameof(block));

			var blockVizualizer = _unityContainer.Resolve<IReportBlockVizualizer<T>>();

			return blockVizualizer;
		}
	}
}