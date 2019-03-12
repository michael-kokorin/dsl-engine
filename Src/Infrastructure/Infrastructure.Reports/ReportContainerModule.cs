namespace Infrastructure.Reports
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Enums;
	using Common.Extensions;
	using Infrastructure.Reports.Blocks;
	using Infrastructure.Reports.Blocks.BoolReportBlock;
	using Infrastructure.Reports.Blocks.Chart;
	using Infrastructure.Reports.Blocks.Container;
	using Infrastructure.Reports.Blocks.Html;
	using Infrastructure.Reports.Blocks.HtmlDoc;
	using Infrastructure.Reports.Blocks.Image;
	using Infrastructure.Reports.Blocks.Iterator;
	using Infrastructure.Reports.Blocks.Label;
	using Infrastructure.Reports.Blocks.Link;
	using Infrastructure.Reports.Blocks.QueryScope;
	using Infrastructure.Reports.Blocks.Table;
	using Infrastructure.Reports.Extensions;
	using Infrastructure.Reports.Generation;
	using Infrastructure.Reports.Generation.Stages;
	using Infrastructure.Reports.Generation.Stages.Query;
	using Infrastructure.Reports.Translation;

	public sealed class ReportContainerModule: IContainerModule
	{
		/// <summary>
		///   Registers binding to the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) => container
			.RegisterType<IReportStorage, ReportStorage>(reuseScope)
			.RegisterType<IReportBuilder, ReportBuilder>(reuseScope)
			.RegisterType<IReportFolderPathStorage, ConfigReportFolderPathStorage>(reuseScope)
			.RegisterType<IReportFileExtensionProvider, ReportFileExtensionProvider>(reuseScope)
			.RegisterType<IReportFileStorage, ReportFileStorage>(reuseScope)
			.RegisterType<IReportAuthorityValidator, ReportAuthorityValidator>(reuseScope)
			.RegisterType<IReportFolderPathProvider, ReportFolderPathProvider>(reuseScope)
			.RegisterType<IReportFolderPathStorage, ConfigReportFolderPathStorage>(reuseScope)
			.RegisterType<IReportBlockVizualizerFabric, ReportBlockVizualizerFabric>(reuseScope)
			.RegisterType<IReportBlockVizualizationManager, ReportBlockVizualizationManager>(reuseScope)
			.RegisterType<IChartScriptProvider, ResourceChartScriptProvider>(reuseScope)
			.RegisterType<IReportQueryLinkDirector, ReportQueryLinkDirector>(reuseScope)

			// report query types
			.RegisterReportQueryExecutor<ReportQuery, ReportQueryExecutor>(reuseScope)
			.RegisterReportQueryExecutor<ReportQueryLink, ReportQueryLinkExecutor>(reuseScope)

			// report generation componenets
			.RegisterType<IReportGenerationPipelineManager, ReportGenerationPipelineManager>(reuseScope)
			.RegisterType<IReportGenerationStageDispatcher, ReportGenerationStageDispatcher>(reuseScope)
			.RegisterGenerationStage<ExecuteQueriesStage>()
			.RegisterGenerationStage<AddDefaultParametersStage>()
			.RegisterGenerationStage<ValidateParametersStage>()
			.RegisterGenerationStage<ReportVizualizationStage>()
			.RegisterGenerationStage<RenderTitleStage>()

			// report block vizualizers
			.RegisterVisualizer<ContainerReportBlock, ContainerReportBlockVizualizer>(reuseScope)
			.RegisterVisualizer<ChartReportBlock, ChartReportBlockVizualizer>(reuseScope)
			.RegisterVisualizer<IteratorReportBlock, IteratorReportBlockVizualizer>(reuseScope)
			.RegisterVisualizer<HtmlDocReportBlock, HtmlDocReportBlockVizualizer>(reuseScope)
			.RegisterVisualizer<LabelReportBlock, LabelReportBlockVizualizer>(reuseScope)
			.RegisterVisualizer<TableReportBlock, TableReportBlockVizualizer>(reuseScope)
			.RegisterVisualizer<ImageReportBlock, ImageReportBlockVizualizer>(reuseScope)
			.RegisterVisualizer<HtmlReportBlock, HtmlReportBlockVizualizer>(reuseScope)
			.RegisterVisualizer<LinkReportBlock, LinkReportBlockVizualizer>(reuseScope)
			.RegisterVisualizer<BoolReportBlock, BoolReportBlockVizualizer>(reuseScope)
			.RegisterVisualizer<QueryScopeReportBlock, QueryScopeReportBlockVizualizer>(reuseScope)

			// report translation components
			.RegisterType<IReportTranslationManager, ReportTranslationManager>(reuseScope)
			.RegisterType<IReportTranslatorFabric, ReportTranslatorFabric>(reuseScope)
			.RegisterTranslator<HtmlReportTranslator>(ReportFileType.Html, reuseScope)
			.RegisterTranslator<PdfReportTranslator>(ReportFileType.Pdf, reuseScope);
	}
}