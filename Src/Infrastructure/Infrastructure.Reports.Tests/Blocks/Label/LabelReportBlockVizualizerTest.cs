namespace Infrastructure.Reports.Tests.Blocks.Label
{
	using System.Collections.Generic;
	using System.IO;
	using System.Web.UI;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Html;
	using Infrastructure.Reports.Blocks;
	using Infrastructure.Reports.Blocks.Label;
	using Infrastructure.Templates;

	using ITemplate = Infrastructure.Templates.ITemplate;

	[TestFixture]
	public sealed class LabelReportBlockVizualizerTest
	{
		private IReportBlockVizualizer<LabelReportBlock> _target;

		private Mock<IHtmlEncoder> _htmlEncoder;

		private Mock<ITemplateBuilder> _templateBuilder;

		private sealed class TestTemplate : ITemplate
		{
			private readonly string _source;

			public TestTemplate(string source)
			{
				_source = source;
			}

			public void Add(IDictionary<string, object> parameters)
			{

			}

			public void Add(string key, object value)
			{

			}

			public string Render() => _source;

			public string GetSource() => _source;
		}

		[SetUp]
		public void SetUp()
		{
			_htmlEncoder = new Mock<IHtmlEncoder>();

			_htmlEncoder.Setup(_ => _.Encode(It.IsAny<string>())).Returns((string s) => s);

			_templateBuilder = new Mock<ITemplateBuilder>();

			_templateBuilder.Setup(_ => _.Build(It.IsAny<string>())).Returns((string s) => new TestTemplate(s));

			_target = new LabelReportBlockVizualizer(_htmlEncoder.Object, _templateBuilder.Object);
		}

		[Test]
		public void ShouldRenderLabel()
		{
			var label = new LabelReportBlock
			{
				Text = "Test label"
			};

			using (var stringWriter = new StringWriter())
			{
				using (var htmlWriter = new HtmlTextWriter(stringWriter))
				{
					_target.Vizualize(htmlWriter, label, null, null, 1);

					var result = stringWriter.ToString();

					result.Should().NotBeNullOrEmpty();
				}
			}
		}
	}
}