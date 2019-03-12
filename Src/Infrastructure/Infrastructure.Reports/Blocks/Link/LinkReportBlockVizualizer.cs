namespace Infrastructure.Reports.Blocks.Link
{
	using System;
	using System.Collections.Generic;
	using System.Web.UI;

	using JetBrains.Annotations;

	using Common.Html;
	using Infrastructure.Templates;

	[UsedImplicitly]
	internal sealed class LinkReportBlockVizualizer : IReportBlockVizualizer<LinkReportBlock>
	{
		private readonly IHtmlEncoder _htmlEncoder;

		private readonly IReportBlockVizualizationManager _blockVizualizationManager;

		private readonly ITemplateBuilder _templateBuilder;

		public LinkReportBlockVizualizer([NotNull] IHtmlEncoder htmlEncoder,
			[NotNull] IReportBlockVizualizationManager blockVizualizationManager,
			[NotNull] ITemplateBuilder templateBuilder)
		{
			if (htmlEncoder == null) throw new ArgumentNullException(nameof(htmlEncoder));
			if (blockVizualizationManager == null) throw new ArgumentNullException(nameof(blockVizualizationManager));
			if (templateBuilder == null) throw new ArgumentNullException(nameof(templateBuilder));

			_htmlEncoder = htmlEncoder;
			_blockVizualizationManager = blockVizualizationManager;
			_templateBuilder = templateBuilder;
		}

		public void Vizualize([NotNull] HtmlTextWriter htmlTextWriter,
			[NotNull] LinkReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
		{
			if (htmlTextWriter == null) throw new ArgumentNullException(nameof(htmlTextWriter));
			if (block == null) throw new ArgumentNullException(nameof(block));

			if (block.Target == null)
				throw new EmptyLinkReportBlockTargetException(block.Id);

			if (block.Child == null)
				throw new EmptyLinkReportBlockChildException(block.Id);

			var template = _templateBuilder.Build(block.Target);

			if (parameterValues != null)
			{
				foreach (var parameterValue in parameterValues)
				{
					template.Add(parameterValue.Key, parameterValue.Value);
				}
			}

			var renderedTemplate = template.Render();

			renderedTemplate = _htmlEncoder.Encode(renderedTemplate);

			htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Href, renderedTemplate);
			htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.A);

			_blockVizualizationManager.Vizualize(
				htmlTextWriter,
				(dynamic) block.Child,
				parameterValues,
				queryResults,
				userId);

			htmlTextWriter.RenderEndTag();
		}
	}
}