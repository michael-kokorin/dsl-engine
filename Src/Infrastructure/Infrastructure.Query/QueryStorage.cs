namespace Infrastructure.Query
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Extensions;
	using Common.Security;
	using Common.Time;
	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Query.Extensions;
	using Infrastructure.Telemetry;
	using Repository.Context;
	using Repository.Repositories;

	using Authorities = Common.Security.Authorities;

	[UsedImplicitly]
	internal sealed class QueryStorage: IQueryStorage
	{
		private readonly IQueryAccessValidator _queryAccessValidator;

		private readonly IQueryModelProcessor _queryModelProcessor;

		private readonly IQueryModelValidator _queryModelValidator;

		private readonly IQueryRepository _queryRepository;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		private readonly ITimeService _timeService;

		private readonly IUserAuthorityValidator _userAuthorityValidator;

		private readonly IUserPrincipal _userPrincipal;

		public QueryStorage(
			[NotNull] IQueryAccessValidator queryAccessValidator,
			[NotNull] IQueryModelProcessor queryModelProcessor,
			[NotNull] IQueryModelValidator queryModelValidator,
			[NotNull] IQueryRepository queryRepository,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider,
			[NotNull] ITimeService timeService,
			[NotNull] IUserPrincipal userPrincipal,
			[NotNull] IUserAuthorityValidator userAuthorityValidator)
		{
			if (queryAccessValidator == null) throw new ArgumentNullException(nameof(queryAccessValidator));
			if (queryModelProcessor == null) throw new ArgumentNullException(nameof(queryModelProcessor));
			if (queryModelValidator == null) throw new ArgumentNullException(nameof(queryModelValidator));
			if (queryRepository == null) throw new ArgumentNullException(nameof(queryRepository));
			if (telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));
			if (timeService == null) throw new ArgumentNullException(nameof(timeService));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));
			if (userAuthorityValidator == null) throw new ArgumentNullException(nameof(userAuthorityValidator));

			_queryAccessValidator = queryAccessValidator;
			_queryModelProcessor = queryModelProcessor;
			_queryRepository = queryRepository;
			_telemetryScopeProvider = telemetryScopeProvider;
			_timeService = timeService;
			_userAuthorityValidator = userAuthorityValidator;
			_queryModelValidator = queryModelValidator;
			_userPrincipal = userPrincipal;
		}

		public long Create(long? projectId, string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			using (var telemetryScope = _telemetryScopeProvider.Create<Queries>(TelemetryOperationNames.Query.Create))
			{
				try
				{
					var canCreateQuery = _userAuthorityValidator.HasUserAuthorities(
						_userPrincipal.Info.Id,
						new[]
						{
							Authorities.UI.Queries.CreateQuery
						},
						projectId);

					if (!canCreateQuery)
						throw new UnauthorizedAccessException();

					var query = new Queries
					{
						Comment = null,
						CreatedById = _userPrincipal.Info.Id,
						CreatedUtc = _timeService.GetUtc(),
						JsonQuery = null,
						ModifiedById = _userPrincipal.Info.Id,
						ModifiedUtc = _timeService.GetUtc(),
						Name = name,
						Privacy = (int) QueryPrivacyType.Private,
						ProjectId = projectId,
						Query = null,
						TargetCultureId = null,
						Visibility = (int) QueryVisibilityType.Closed
					};

					telemetryScope.SetEntity(query);

					_queryRepository.Insert(query);

					_queryRepository.Save();

					telemetryScope.WriteSuccess();

					return query.Id;
				}
				catch (Exception ex)
				{
					telemetryScope.WriteException(ex);

					throw;
				}
			}
		}

		public QueryInfo Get(long queryId)
		{
			var query = _queryRepository.GetById(queryId);

			if (query == null) throw new QueryDoesNotExistsException(queryId);

			if (!_queryAccessValidator.IsCanView(query, _userPrincipal.Info.Id))
				throw new UnauthorizedAccessException();

			return ProcessQuery(query);
		}

		public QueryInfo Get(long? projectId, [NotNull] string name)
		{
			if (name == null) throw new ArgumentNullException(nameof(name));

			var query = _queryRepository.Get(name, projectId).SingleOrDefault();

			if (query == null) throw new QueryDoesNotExistsException(name);

			if (!_queryAccessValidator.IsCanView(query, _userPrincipal.Info.Id))
				throw new UnauthorizedAccessException();

			return ProcessQuery(query);
		}

		public QueryInfo GetMy(long? projectId, string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			var query = _queryRepository.Get(_userPrincipal.Info.Id, name, projectId);

			return ProcessQuery(query);
		}

		public void Update(
			long queryId,
			string textQuery,
			string name,
			string comment,
			QueryPrivacyType privacyType,
			QueryVisibilityType visibilityType)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			using (var telemetryScope = _telemetryScopeProvider.Create<Queries>(TelemetryOperationNames.Query.Update))
			{
				try
				{
					var query = _queryRepository.GetById(queryId);

					if (query == null)
						throw new QueryDoesNotExistsException(queryId);

					telemetryScope.SetEntity(query);

					var canEditQuery = _queryAccessValidator.IsCanEdit(query, _userPrincipal.Info.Id);

					if (!canEditQuery)
						throw new UnauthorizedAccessException();

					if (!string.IsNullOrEmpty(textQuery))
					{
						var queryModel = _queryModelProcessor.FromText(textQuery, query.ProjectId, query.IsSystem);

						_queryModelValidator.Validate(queryModel);

						query.JsonQuery = queryModel.ToJson();
					}
					else
					{
						query.JsonQuery = null;
					}

					query.Comment = comment;
					query.ModifiedUtc = _timeService.GetUtc();
					query.Name = name;
					query.Privacy = (int) privacyType;
					query.Query = textQuery;
					query.Visibility = (int) visibilityType;

					_queryRepository.Save();

					telemetryScope.WriteSuccess();
				}
				catch (Exception ex)
				{
					telemetryScope.WriteException(ex);

					throw;
				}
			}
		}

		public void Update(
			long queryId,
			DslDataQuery dslQuery,
			string name,
			string comment,
			QueryPrivacyType privacyType,
			QueryVisibilityType visibilityType)
		{
			using (var telemetryScope = _telemetryScopeProvider.Create<Queries>(TelemetryOperationNames.Query.Update))
			{
				try
				{
					if (string.IsNullOrEmpty(name))
						throw new ArgumentNullException(nameof(name));

					var query = _queryRepository.GetById(queryId);

					if (query == null)
						throw new QueryDoesNotExistsException(queryId);

					telemetryScope.SetEntity(query);

					var canEditQuery = _queryAccessValidator.IsCanEdit(query, _userPrincipal.Info.Id);

					if (!canEditQuery)
						throw new UnauthorizedAccessException();

					string dslQueryText = null;

					if (dslQuery != null)
					{
						dslQueryText = _queryModelProcessor.ToText(dslQuery, query.ProjectId, query.IsSystem);
					}

					_queryModelValidator.Validate(dslQuery);

					query.Comment = comment;
					query.JsonQuery = dslQuery.ToJson();
					query.ModifiedUtc = _timeService.GetUtc();
					query.Name = name;
					query.Privacy = (int) privacyType;
					query.Query = dslQueryText;
					query.Visibility = (int) visibilityType;

					_queryRepository.Save();

					telemetryScope.WriteSuccess();
				}
				catch (Exception ex)
				{
					telemetryScope.WriteException(ex);

					throw;
				}
			}
		}

		private QueryInfo ProcessQuery(Queries query)
		{
			if (!_queryAccessValidator.IsCanView(query, _userPrincipal.Info.Id))
				throw new UnauthorizedAccessException();

			if ((query.JsonQuery != null) || string.IsNullOrEmpty(query.Query))
				return query.ToDto();

			var queryModel = _queryModelProcessor.FromText(query.Query, query.ProjectId, query.IsSystem);

			query.JsonQuery = queryModel.ToJson();

			return query.ToDto();
		}
	}
}