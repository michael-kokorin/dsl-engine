namespace Infrastructure.Reports.Extensions
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Enums;
	using Common.Extensions;
	using Infrastructure.Reports.Blocks;
	using Infrastructure.Reports.Generation.Stages;
	using Infrastructure.Reports.Generation.Stages.Query;
	using Infrastructure.Reports.Translation;

	internal static class ContainerExtension
	{
		public static IUnityContainer RegisterGenerationStage<T>(this IUnityContainer container)
			where T : class, IReportGenerationStage => container.RegisterType<T>(ReuseScope.PerResolve);

		public static IUnityContainer RegisterTranslator<T>(this IUnityContainer container,
			ReportFileType reportFileType,
			ReuseScope reuseScope)
			where T : IReportTranslator => container.RegisterType<IReportTranslator, T>(reportFileType.ToString(), reuseScope);

		public static IUnityContainer RegisterVisualizer<TBlock, TVizualizer>(this IUnityContainer container,
			ReuseScope reuseScope)
			where TBlock : class, IReportBlock
			where TVizualizer : class, IReportBlockVizualizer<TBlock> =>
				container.RegisterType<IReportBlockVizualizer<TBlock>, TVizualizer>(reuseScope);

		public static IUnityContainer RegisterReportQueryExecutor<TQuery, TExecutor>(
			this IUnityContainer container,
			ReuseScope reuseScope)
			where TQuery: class, IReportQuery
			where TExecutor: IReportQueryLinkExecutor<TQuery> =>
			container.RegisterType<IReportQueryLinkExecutor<TQuery>, TExecutor>(reuseScope);
	}
}