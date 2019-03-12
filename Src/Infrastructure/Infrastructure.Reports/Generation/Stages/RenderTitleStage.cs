namespace Infrastructure.Reports.Generation.Stages
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Templates;

	[UsedImplicitly]
	internal sealed class RenderTitleStage : ReportGenerationStage
	{
		private readonly ITemplateBuilder _templateBuilder;

		public RenderTitleStage([NotNull] ITemplateBuilder templateBuilder)
		{
			if (templateBuilder == null) throw new ArgumentNullException(nameof(templateBuilder));

			_templateBuilder = templateBuilder;
		}

		protected override void ExecuteStage([NotNull] ReportBundle reportBundle)
		{
			if (reportBundle == null) throw new ArgumentNullException(nameof(reportBundle));

			if (string.IsNullOrEmpty(reportBundle.Rule?.ReportTitle))
			{
				reportBundle.Title = reportBundle.Report.DisplayName;

				return;
			}

			var titleTemplate = _templateBuilder.Build(reportBundle.Rule.ReportTitle);

			foreach (var parameter in reportBundle.ParameterValues)
			{
				titleTemplate.Add(parameter.Key, parameter.Value);
			}

			foreach (var queryResult in reportBundle.QueryResults)
			{
				titleTemplate.Add(queryResult.Key, queryResult.Result.Items);
			}

			reportBundle.Title = titleTemplate.Render();
		}
	}
}