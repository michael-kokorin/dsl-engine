namespace Modules.Core.Services.Query
{
	using System;

	using JetBrains.Annotations;

	using Common.Command;
	using Common.Extensions;
	using Common.Logging;
	using Common.Query;
	using Modules.Core.Contracts.Dto;
	using Modules.Core.Contracts.Query;
	using Modules.Core.Contracts.Query.Dto;
	using Modules.Core.Contracts.UI.Dto.Data;
	using Modules.Core.Services.Query.Commands;
	using Modules.Core.Services.Query.DataQueries;

	public sealed class QueryService : IQueryService
	{
		private readonly IDataQueryDispatcher _dataQueryDispatcher;

		private readonly ICommandDispatcher _commandDispatcher;

		public QueryService([NotNull] ICommandDispatcher commandDispatcher,
			[NotNull] IDataQueryDispatcher dataQueryDispatcher)
		{
			if (commandDispatcher == null) throw new ArgumentNullException(nameof(commandDispatcher));
			if (dataQueryDispatcher == null) throw new ArgumentNullException(nameof(dataQueryDispatcher));

			_commandDispatcher = commandDispatcher;
			_dataQueryDispatcher = dataQueryDispatcher;
		}

		[LogMethod]
		public TableDto Execute(long queryId, long userId, string parameters)
		{
			QueryParameterValue[] queryParameters = null;

			if (parameters != null)
			{
				queryParameters = parameters.FromJson<QueryParameterValue[]>();
			}

			var query = new GetQueryResultsQuery(queryId, userId, queryParameters);

			var result = _dataQueryDispatcher.Process<GetQueryResultsQuery, TableDto>(query);

			return result;
		}

		[LogMethod]
		public QueryDto Get(long queryId)
		{
			var query = new QueryByIdQuery(queryId);

			return _dataQueryDispatcher.Process<QueryByIdQuery, QueryDto>(query);
		}

		[LogMethod]
		public ReferenceItemDto[] GetPrivacyReference()
		{
			var query = new GetQueryPrivacyQuery();

			return _dataQueryDispatcher.Process<GetQueryPrivacyQuery, ReferenceItemDto[]>(query);
		}

		[LogMethod]
		public ReferenceItemDto[] GetSortDirections()
		{
			var query = new GetSortDirectionsQuery();

			return _dataQueryDispatcher.Process<GetSortDirectionsQuery, ReferenceItemDto[]>(query);
		}

		[LogMethod]
		public ReferenceItemDto[] GetFilterConditions()
		{
			var query = new GetQueryFilterConditionsQuery();

			return _dataQueryDispatcher.Process<GetQueryFilterConditionsQuery, ReferenceItemDto[]>(query);
		}

		[LogMethod]
		public ReferenceItemDto[] GetFilterOperations()
		{
			var query = new GetQueryFilterOperationsQuery();

			return _dataQueryDispatcher.Process<GetQueryFilterOperationsQuery, ReferenceItemDto[]>(query);
		}

		[LogMethod]
		public DataSourceDto[] GetDataSources(string projectId)
		{
			var projectIdVal = string.IsNullOrEmpty(projectId) ? (long?)null : Convert.ToInt64(projectId);

			var query = new DataSourcesQuery(projectIdVal);

			return _dataQueryDispatcher.Process<DataSourcesQuery, DataSourceDto[]>(query);
		}

		[LogMethod]
		public DataSourceFieldDto[] GetDataSourceFields(string dataSourceKey, string projectId)
		{
			var projectIdVal = string.IsNullOrEmpty(projectId) ? (long?) null : Convert.ToInt64(projectId);

			var query = new GetDataSourceFieldsQuery(dataSourceKey, projectIdVal);

			return _dataQueryDispatcher.Process<GetDataSourceFieldsQuery, DataSourceFieldDto[]>(query);
		}

		[LogMethod]
		public TableDto GetList()
		{
			var query = new GetQueriesListQuery();

			return _dataQueryDispatcher.Process<GetQueriesListQuery, TableDto>(query);
		}

		[LogMethod]
		public QueryDto Create(long projectId, string name)
		{
			var command = new CreateQueryCommand(name, projectId);

			_commandDispatcher.Handle(command);

			var query = new QueryByNameQuery(name, projectId);

			var queryDto = _dataQueryDispatcher.Process<QueryByNameQuery, QueryDto>(query);

			return queryDto;
		}

		[LogMethod]
		public bool IsCanEdit(long queryId)
		{
			var query = new CanEditQueryQuery(queryId);

			return _dataQueryDispatcher.Process<CanEditQueryQuery, bool>(query);
		}

		[LogMethod]
		public QueryDto Update(QueryDto query)
		{
			var command = new UpdateQueryCommand(query);

			_commandDispatcher.Handle(command);

			var dataQuery = new QueryByIdQuery(query.Id);

			var queryDto = _dataQueryDispatcher.Process<QueryByIdQuery, QueryDto>(dataQuery);

			return queryDto;
		}

		[LogMethod]
		public string ConvertToText(string queryModel)
		{
			var query = new GetQueryTextQuery(queryModel);

			return _dataQueryDispatcher.Process<GetQueryTextQuery, string>(query);
		}

		[LogMethod]
		public string ConvertToModel(string query)
		{
			var queryQuery = new GetQueryModelQuery(query);

			return _dataQueryDispatcher.Process<GetQueryModelQuery, string>(queryQuery);
		}
	}
}