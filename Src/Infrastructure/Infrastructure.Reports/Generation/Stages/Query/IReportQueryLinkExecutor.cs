namespace Infrastructure.Reports.Generation.Stages.Query
{
	using System.Collections.Generic;

	using Infrastructure.Engines.Query.Result;

	internal interface IReportQueryLinkExecutor<in T> where T: class, IReportQuery
	{
		QueryResult Execute(long userId, T reportQuery, IEnumerable<KeyValuePair<string, string>> parameters);
	}
}