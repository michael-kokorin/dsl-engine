namespace Infrastructure.Reports.Generation.Stages
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.Reports.Generation.Stages.Query;

	[UsedImplicitly]
	internal sealed class ExecuteQueriesStage : ReportGenerationStage
	{
		private readonly IReportQueryLinkDirector _reportQueryLinkDirector;

		public ExecuteQueriesStage([NotNull] IReportQueryLinkDirector reportQueryLinkDirector)
		{
			if (reportQueryLinkDirector == null) throw new ArgumentNullException(nameof(reportQueryLinkDirector));

			_reportQueryLinkDirector = reportQueryLinkDirector;
		}

		protected override void ExecuteStage(ReportBundle reportBundle)
		{
			var queryResults = new List<ReportQueryResult>();

			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (var reportQuery in reportBundle.Rule.QueryLinks)
			{
				var parameters = GetReportDefaultParameters(reportBundle);

				var queryResult = _reportQueryLinkDirector.Execute(reportBundle.TargetUserId, (dynamic) reportQuery, parameters);

				queryResults.Add(new ReportQueryResult
				{
					Key = reportQuery.Key,
					Result = queryResult
				});
			}

			reportBundle.QueryResults = queryResults;
		}

		private static IEnumerable<KeyValuePair<string, string>> GetReportDefaultParameters(ReportBundle reportBundle)
		{
			var parameters = reportBundle
				.ParameterValues
				?.Select(_ => new KeyValuePair<string, string>(_.Key, _.Value.ToString()));
			return parameters;
		}
	}
}