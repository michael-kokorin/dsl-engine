namespace Infrastructure.Reports.Tests.Blocks.Table
{
	using System.IO;
	using System.Web.UI;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Engines.Query.Result;
	using Infrastructure.Reports.Blocks;
	using Infrastructure.Reports.Blocks.Table;

	[TestFixture]
	public sealed class TableReportBlockVizualizerTest
	{
		private IReportBlockVizualizer<TableReportBlock> _target;

		private Mock<IReportBlockVizualizationManager> _blockVizualizationManager;

		[SetUp]
		public void SetUp()
		{
			_blockVizualizationManager = new Mock<IReportBlockVizualizationManager>();

			_target = new TableReportBlockVizualizer(_blockVizualizationManager.Object);
		}

		[Test]
		public void ShouldBeSerializable()
		{
			var tableBlock = new TableReportBlock
			{
				BorderPx = 4,
				QueryKey = "QQ1"
			};

			var serialized = tableBlock.ToJson();

			var deserialized = serialized.FromJson<TableReportBlock>();

			deserialized.ShouldBeEquivalentTo(tableBlock);
		}

		[Test]
		public void ShouldVizualizeTableBlock()
		{
			const string queryKey = "testQuery";

			var block = new TableReportBlock
			{
				QueryKey = queryKey
			};

			var query = new ReportQueryResult
			{
				Key = queryKey,
				Result = new QueryResult
				{
					Columns = new[]
					{
						new QueryResultColumn("First", "First column", "Test desc"),
						new QueryResultColumn("Second", "Second column", "Second column desc")
					},
					Exceptions = null,
					Items = new[]
					{
						new QueryResultItem
						{
							Value = new
							{
								First = "1",
								Second = "2"
							}
						},
						new QueryResultItem
						{
							Value = new
							{
								First = 3,
								Second = 4
							}
						}
					}
				}
			};

			var queries = new[] {query};

			using (var stringWriter = new StringWriter())
			{
				using (var htmlTextWriter = new HtmlTextWriter(stringWriter))
				{
					_target.Vizualize(htmlTextWriter, block, null, queries, 1);

					var result = stringWriter.ToString();

					result.Should().NotBeNullOrEmpty();
				}
			}
		}
	}
}