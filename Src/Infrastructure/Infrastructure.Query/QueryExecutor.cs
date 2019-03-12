namespace Infrastructure.Query
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Security;
	using Infrastructure.Engines;
	using Infrastructure.Engines.Dsl;
	using Infrastructure.Engines.Query.Result;
	using Infrastructure.Query.Evaluation;
	using Infrastructure.Telemetry;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class QueryExecutor: IQueryExecutor
	{
		private readonly IDslDataQueryEvaluator _dslDataQueryEvaluator;

		private readonly IDslParser _dslParser;

		private readonly IQueryBuilder _queryBuilder;

		private readonly IQueryRepository _queryRepository;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		private readonly IUserPrincipal _userPrincipal;

		public QueryExecutor(
			[NotNull] IDslParser dslParser,
			[NotNull] IQueryRepository queryRepository,
			[NotNull] IDslDataQueryEvaluator dslDataQueryEvaluator,
			[NotNull] IQueryBuilder queryBuilder,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider,
			[NotNull] IUserPrincipal userPrincipal)
		{
			if (dslParser == null) throw new ArgumentNullException(nameof(dslParser));
			if (queryRepository == null) throw new ArgumentNullException(nameof(queryRepository));
			if (dslDataQueryEvaluator == null) throw new ArgumentNullException(nameof(dslDataQueryEvaluator));
			if (queryBuilder == null) throw new ArgumentNullException(nameof(queryBuilder));
			if (telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_dslParser = dslParser;
			_queryRepository = queryRepository;
			_dslDataQueryEvaluator = dslDataQueryEvaluator;
			_queryBuilder = queryBuilder;
			_telemetryScopeProvider = telemetryScopeProvider;
			_userPrincipal = userPrincipal;
		}

		public QueryResult Execute(long queryId, long? userId = null, params KeyValuePair<string, string>[] parameters)
		{
			var query = _queryRepository.GetById(queryId);

			if (query == null)
				throw new QueryDoesNotExistsException(queryId);

			return query.Query == null ? new QueryResult() : ExecutePlainTextQuery(userId, query, parameters);
		}

		public QueryResult Execute(string query, long? userId = null, params KeyValuePair<string, string>[] parameters) =>
			ExecutePlainTextQuery(
				userId,
				new Queries
				{
					Query = query
				},
				parameters);

		private static string ApplyQueryParameters(string query, params KeyValuePair<string, string>[] parameters) =>
			parameters == null
				? query
				: parameters.Aggregate(query, (current, pair) => current.Replace("{" + pair.Key + "}", pair.Value));

		private QueryResult ExecutePlainTextQuery(long? userId, Queries query, KeyValuePair<string, string>[] parameters)
		{
			userId = userId ?? _userPrincipal.Info.Id;

			var queryResult = new QueryResult();

			using (var telemetryScope = _telemetryScopeProvider.Create<Queries>(TelemetryOperationNames.Query.Execute))
			{
				telemetryScope.SetEntity(query);

				try
				{
					var queryText = ApplyQueryParameters(query.Query, parameters);

					var queryExpr = _dslParser.DataQueryParse(queryText);

					queryResult.Columns = _dslDataQueryEvaluator.Evaluate(queryExpr, userId.Value);

					queryResult.Items = _queryBuilder.ExecuteTable(queryExpr).ToArray(); // generalize array

					telemetryScope.WriteSuccess();
				}
				catch (DataQueryCompilationException ex)
				{
					telemetryScope.WriteException(ex);

					queryResult.Exceptions = ex.Errors
						.Select(
							_ => new QueryException
							{
								Message = $"{_.Item1}: {_.Item2}"
							});
				}
				catch (Exception ex)
				{
					telemetryScope.WriteException(ex);

					queryResult.Exceptions = new[]
					{
						new QueryException
						{
							Message = ex.Message,
							StackTrace = ex.StackTrace
						}
					};
				}
			}

			return queryResult;
		}
	}
}