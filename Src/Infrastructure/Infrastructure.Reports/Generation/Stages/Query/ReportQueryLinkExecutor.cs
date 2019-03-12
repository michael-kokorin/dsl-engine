namespace Infrastructure.Reports.Generation.Stages.Query
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Query.Result;
	using Infrastructure.Query;

	[UsedImplicitly]
	internal sealed class ReportQueryLinkExecutor : IReportQueryLinkExecutor<ReportQueryLink>
	{
		private readonly IQueryExecutor _queryExecutor;

		public ReportQueryLinkExecutor([NotNull] IQueryExecutor queryExecutor)
		{
			if (queryExecutor == null) throw new ArgumentNullException(nameof(queryExecutor));

			_queryExecutor = queryExecutor;
		}

		public QueryResult Execute(long userId, ReportQueryLink reportQuery, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			List<KeyValuePair<string, string>> reportQueryParameters = null;

			// ReSharper disable once InvertIf
			if ((parameters != null) && (reportQuery.Parameters != null))
			{
				reportQueryParameters = new List<KeyValuePair<string, string>>(parameters);

				// ReSharper disable once LoopCanBeConvertedToQuery
				foreach (var queryParameter in reportQuery.Parameters)
				{
					reportQueryParameters.Add(new KeyValuePair<string, string>(queryParameter.Key, queryParameter.Value));
				}
			}

			return _queryExecutor.Execute(reportQuery.QueryId, userId, reportQueryParameters?.ToArray());
		}
	}
}