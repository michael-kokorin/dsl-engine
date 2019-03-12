namespace Infrastructure.Engines
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Data;
	using Infrastructure.Engines.Dsl;
	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Query.Result;

	[UsedImplicitly]
	public sealed class QueryBuilder : IQueryBuilder
	{
		private readonly IDataQueryExecutor _executor;

		private readonly IDslParser _parser;

		private readonly IDataQueryExpressionTranslator _translator;

		private readonly IQueryEntityNameTranslator _nameTranslator;

		private readonly IDataSourceProvider _dataSourceProvider;

		public QueryBuilder(
			IDataQueryExecutor executor,
			IDslParser parser,
			IDataQueryExpressionTranslator translator,
			IQueryEntityNameTranslator nameTranslator,
			IDataSourceProvider dataSourceProvider)
		{
			_executor = executor;
			_parser = parser;
			_translator = translator;
			_nameTranslator = nameTranslator;
			_dataSourceProvider = dataSourceProvider;
		}

		private static string ApplyQueryParameters(string query, params KeyValuePair<string, string>[] parameters) =>
			parameters.Aggregate(query, (current, pair) => current.Replace("{" + pair.Key + "}", pair.Value));

		private object Execute(string query, KeyValuePair<string, string>[] parameters,
			Action<DslDataQuery> exprVerificator)
		{
			var parametrizedQuery = ApplyQueryParameters(query, parameters);

			var expr = _parser.DataQueryParse(parametrizedQuery);

			exprVerificator(expr);

			return ExecuteExpr(expr);
		}

		private object ExecuteExpr(DslDataQuery query)
		{
			var linqQuery = _translator.Translate(query);
			var entityType = _nameTranslator.GetEntityType(query.QueryEntityName);
			var source = _dataSourceProvider.GetDataSource(entityType);

			var result = _executor.Execute(source, entityType, linqQuery);

			return result;
		}

		public IEnumerable<QueryResultItem> ExecuteTable(string query, params KeyValuePair<string, string>[] parameters)
			=>
				Execute(query, parameters, expr =>
				{
					if (expr.TableKey == null)
					{
						throw new InvalidOperationException();
					}
				}) as IEnumerable<QueryResultItem>;

		public IEnumerable<QueryResultItem> ExecuteTable(DslDataQuery query)
		{
			query.IsTableRenderRequired = true;

			return ExecuteExpr(query) as IEnumerable<QueryResultItem>;
		}

		public IEnumerable ExecuteEnumerable(string query, params KeyValuePair<string, string>[] parameters) =>
			Execute(query, parameters, expr =>
			{
				if (expr.TakeFirst || expr.TakeFirstOrDefault)
				{
					throw new InvalidOperationException();
				}
			}) as IEnumerable;

		public object Execute(string query, params KeyValuePair<string, string>[] parameters) =>
			Execute(query, parameters, expr => { });
	}
}