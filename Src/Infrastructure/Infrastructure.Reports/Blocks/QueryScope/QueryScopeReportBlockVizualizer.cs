namespace Infrastructure.Reports.Blocks.QueryScope
{
	using System;
	using System.Collections.Generic;
	using System.Web.UI;

	using JetBrains.Annotations;

	using Infrastructure.Reports.Generation.Stages.Query;
	using Infrastructure.Templates;

	[UsedImplicitly]
	internal sealed class QueryScopeReportBlockVizualizer: IReportBlockVizualizer<QueryScopeReportBlock>
	{
		private readonly IReportBlockVizualizationManager _reportBlockVizualizationManager;

		private readonly IReportQueryLinkDirector _reportQueryLinkDirector;

		private readonly ITemplateBuilder _templateBuilder;

		public QueryScopeReportBlockVizualizer(
			[NotNull] IReportBlockVizualizationManager reportBlockVizualizationManager,
			[NotNull] IReportQueryLinkDirector reportQueryLinkDirector,
			[NotNull] ITemplateBuilder templateBuilder)
		{
			if (reportBlockVizualizationManager == null)
				throw new ArgumentNullException(nameof(reportBlockVizualizationManager));
			if (reportQueryLinkDirector == null) throw new ArgumentNullException(nameof(reportQueryLinkDirector));
			if (templateBuilder == null) throw new ArgumentNullException(nameof(templateBuilder));

			_reportBlockVizualizationManager = reportBlockVizualizationManager;
			_reportQueryLinkDirector = reportQueryLinkDirector;
			_templateBuilder = templateBuilder;
		}

		public void Vizualize(
			[NotNull] HtmlTextWriter htmlTextWriter,
			[NotNull] QueryScopeReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
		{
			if (htmlTextWriter == null) throw new ArgumentNullException(nameof(htmlTextWriter));
			if (block == null) throw new ArgumentNullException(nameof(block));

			if (block.Child == null)
				throw new Exception("Empty child block"); // TODO: normal exception

			var queryParameters = GetQueryParameters(block, parameterValues);

			var scopeQueryResult = _reportQueryLinkDirector.Execute(
				userId,
				(dynamic) block.Query,
				queryParameters);

			var childQueries = new List<ReportQueryResult>();

			if (queryResults != null)
			{
				// ReSharper disable once LoopCanBeConvertedToQuery
				foreach (var queryResult in queryResults)
				{
					childQueries.Add(queryResult);
				}
			}

			childQueries.Add(
				new ReportQueryResult
				{
					Key = block.Query.Key,
					Result = scopeQueryResult
				});

			_reportBlockVizualizationManager.Vizualize(
				htmlTextWriter,
				(dynamic) block.Child,
				parameterValues,
				childQueries,
				userId);
		}

		private IEnumerable<KeyValuePair<string, string>> GetQueryParameters(QueryScopeReportBlock block, IReadOnlyDictionary<string, object> parameterValues)
		{
			var queryParameters = new List<KeyValuePair<string, string>>();

			if (block.Parameters == null) return queryParameters;

			foreach (var parameter in block.Parameters)
			{
				var template = _templateBuilder.Build(parameter.Template);

				foreach (var parameterValue in parameterValues)
				{
					template.Add(parameterValue.Key, parameterValue.Value);
				}

				try
				{
					var parameterValue = template.Render();

					queryParameters.Add(new KeyValuePair<string, string>(parameter.Key, parameterValue));
				}
				catch (Exception)
				{
					// TODO: typed exception
					throw new Exception($"Failed to render report query block parameter template. BlockId='{block.Id}'");
				}
			}

			return queryParameters;
		}
	}
}