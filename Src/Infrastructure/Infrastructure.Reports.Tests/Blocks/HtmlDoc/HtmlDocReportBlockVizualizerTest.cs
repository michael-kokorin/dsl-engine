namespace Infrastructure.Reports.Tests.Blocks.HtmlDoc
{
	using System.IO;
	using System.Web.UI;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.Reports.Blocks;
	using Infrastructure.Reports.Blocks.Chart;
	using Infrastructure.Reports.Blocks.HtmlDoc;
	using Infrastructure.Reports.Blocks.Label;

	[TestFixture]
	public sealed class HtmlDocReportBlockVizualizerTest
	{
		private IReportBlockVizualizer<HtmlDocReportBlock> _target;

		private Mock<IChartScriptProvider> _chartScriptProvider;

		private Mock<IReportBlockVizualizationManager> _blockVizualizationManager;

		private const string Script = "#script$";

		[SetUp]
		public void SetUp()
		{
			_chartScriptProvider = new Mock<IChartScriptProvider>();

			_chartScriptProvider.Setup(_ => _.GetScript()).Returns(Script);

			_blockVizualizationManager = new Mock<IReportBlockVizualizationManager>();

			_target = new HtmlDocReportBlockVizualizer(_chartScriptProvider.Object, _blockVizualizationManager.Object);
		}

		[Test]
		public void ShouldRenderHtmcDocBlockWithHeaders()
		{
			var htmlDocBlock = new HtmlDocReportBlock
			{
				Child = new LabelReportBlock(),
				WithHeader = true
			};

			using (var stringWriter = new StringWriter())
			{
				using (var htmlTextWriter = new HtmlTextWriter(stringWriter))
				{
					_target.Vizualize(htmlTextWriter, htmlDocBlock, null, null, 1);

					htmlTextWriter.ToString().Should().NotBeNullOrEmpty();
				}
			}
		}

		[Test]
		public void ShouldRenderHtmlDocBlockWithoutHeaders()
		{
			var htmlDocBlock = new HtmlDocReportBlock
			{
				Child = new LabelReportBlock(),
				WithHeader = false
			};

			using (var stringWriter = new StringWriter())
			{
				using (var htmlTextWriter = new HtmlTextWriter(stringWriter))
				{
					_target.Vizualize(htmlTextWriter, htmlDocBlock, null, null, 1);

					var result = htmlTextWriter.ToString();

					result.Should().NotBeNullOrEmpty();
				}
			}
		}
	}
}