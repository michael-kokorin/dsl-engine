namespace Infrastructure.Reports.Generation.Stages.Query
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Query.Result;
	using Infrastructure.Query;

	[UsedImplicitly]
	internal sealed class ReportQueryExecutor: IReportQueryLinkExecutor<ReportQuery>
	{
		private readonly IQueryExecutor _queryExecutor;

		public ReportQueryExecutor([NotNull] IQueryExecutor queryExecutor)
		{
			if (queryExecutor == null) throw new ArgumentNullException(nameof(queryExecutor));

			_queryExecutor = queryExecutor;
		}

		public QueryResult Execute(
			long userId,
			[NotNull] ReportQuery reportQuery,
			IEnumerable<KeyValuePair<string, string>> parameters)
		{
			if (reportQuery == null) throw new ArgumentNullException(nameof(reportQuery));

			if (string.IsNullOrEmpty(reportQuery.Text))
				throw new Exception($"Empry report query. Query key='{reportQuery.Key}'");

			return _queryExecutor.Execute(reportQuery.Text, userId, parameters.ToArray());
		}
	}
}