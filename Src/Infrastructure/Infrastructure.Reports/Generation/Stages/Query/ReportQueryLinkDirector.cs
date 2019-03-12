namespace Infrastructure.Reports.Generation.Stages.Query
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Infrastructure.Engines.Query.Result;

	[UsedImplicitly]
	internal sealed class ReportQueryLinkDirector: IReportQueryLinkDirector
	{
		private readonly IUnityContainer _unityContainer;

		public ReportQueryLinkDirector([NotNull] IUnityContainer unityContainer)
		{
			if (unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));

			_unityContainer = unityContainer;
		}

		public QueryResult Execute<T>(long userId,
									  [NotNull] T reportQuery,
									  IEnumerable<KeyValuePair<string, string>> parameters)
			where T: class, IReportQuery
		{
			if (reportQuery == null) throw new ArgumentNullException(nameof(reportQuery));

			var reportQueryLinkExecutor = _unityContainer.Resolve<IReportQueryLinkExecutor<T>>();

			return reportQueryLinkExecutor.Execute(userId, reportQuery, parameters);
		}
	}
}