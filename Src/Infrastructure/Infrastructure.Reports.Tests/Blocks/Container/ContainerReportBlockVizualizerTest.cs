namespace Infrastructure.Reports.Tests.Blocks.Container
{
	using System.Collections.Concurrent;
	using System.IO;
	using System.Web.UI;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Reports.Blocks;
	using Infrastructure.Reports.Blocks.Container;
	using Infrastructure.Reports.Blocks.Label;
	using Infrastructure.Reports.Generation.Stages;

	[TestFixture]
	public sealed class ContainerReportBlockVizualizerTest
	{
		private IReportBlockVizualizer<ContainerReportBlock> _target;

		private Mock<IReportBlockVizualizationManager> _reportBlockVizualizationManager;

		[SetUp]
		public void SetUp()
		{
			_reportBlockVizualizationManager = new Mock<IReportBlockVizualizationManager>();

			_target = new ContainerReportBlockVizualizer(_reportBlockVizualizationManager.Object);
		}

		[Test]
		public void ShoudBeSerializable()
		{
			var containerBlock = new ContainerReportBlock
			{
				Childs = new IReportBlock[]
				{
					new LabelReportBlock
					{
						FontStyle = new LabelFontStyle(46),
						Text = "Halo"
					}
				},
				Orientation = ContainerOrientation.Horizontal
			};

			var serialized = containerBlock.ToJson();

			var result = serialized.FromJson<ContainerReportBlock>();

			result.ShouldBeEquivalentTo(containerBlock);
		}

		[Test]
		public void ShouldProcessBlocks()
		{
			var labelBlock = new LabelReportBlock();

			var containerBlock = new ContainerReportBlock
			{
				Childs = new IReportBlock[]
				{
					labelBlock,
					labelBlock
				},
				Orientation = ContainerOrientation.Vertical
			};

			using (var stringWriter = new StringWriter())
			{
				using (var htmlTextWriter = new HtmlTextWriter(stringWriter))
				{
					_target.Vizualize(htmlTextWriter, containerBlock, null, null, 1);

					htmlTextWriter.ToString().Should().NotBeNullOrEmpty();
				}
			}
		}

		[Test]
		public void ShouldProcessBlocksAsTable()
		{
			var labelBlock = new LabelReportBlock();

			var containerBlock = new ContainerReportBlock
			{
				Childs = new IReportBlock[]
				{
					labelBlock,
					labelBlock
				},
				Orientation = ContainerOrientation.Horizontal
			};

			var paramDict = new ConcurrentDictionary<string, object>();

			paramDict.AddOrUpdate(DefaultReportParameters.ContainerUseTable, true, (s, o) => true);

			using (var stringWriter = new StringWriter())
			{
				using (var htmlTextWriter = new HtmlTextWriter(stringWriter))
				{
					_target.Vizualize(htmlTextWriter, containerBlock, paramDict, null, 1);
				}

				var result = stringWriter.ToString();

				result.Should().NotBeNullOrEmpty();
			}
		}

		[Test]
		public void ShouldVizualizeWithEmptyChildBlocks()
		{
			var containerBlock = new ContainerReportBlock
			{
				Childs = null
			};

			using (var stringWriter = new StringWriter())
			{
				using (var htmlTextWriter = new HtmlTextWriter(stringWriter))
				{
					_target.Vizualize(htmlTextWriter, containerBlock, null, null, 1);

					stringWriter.ToString().Should().NotBeNullOrEmpty();
				}
			}
		}
	}
}