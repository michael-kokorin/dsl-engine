namespace Infrastructure.Reports.Generation.Stages.Query
{
	using System.Collections.Generic;

	using Infrastructure.Engines.Query.Result;

	internal interface IReportQueryLinkDirector
	{
		QueryResult Execute<T>(long userId, T reportQuery, IEnumerable<KeyValuePair<string, string>> parameters)
			where T: class, IReportQuery;
	}
}