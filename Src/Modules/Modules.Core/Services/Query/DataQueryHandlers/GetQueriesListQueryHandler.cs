namespace Modules.Core.Services.Query.DataQueryHandlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Query;
	using Common.Security;
	using Infrastructure.Engines.Query.Result;
	using Modules.Core.Contracts.UI.Dto.Data;
	using Modules.Core.Services.Query.DataQueries;
	using Modules.Core.Services.UI.Renderers;
	using Repository.Repositories;

	[UsedImplicitly]
	// ReSharper disable LoopCanBePartlyConvertedToQuery
	internal sealed class GetQueriesListQueryHandler : IDataQueryHandler<GetQueriesListQuery, TableDto>
	{
		private readonly IQueryRepository _queryRepository;

		private readonly IUserAuthorityValidator _userAuthorityValidator;

		private readonly IUserPrincipal _userPrincipal;

		public GetQueriesListQueryHandler([NotNull] IQueryRepository queryRepository,
			[NotNull] IUserAuthorityValidator userAuthorityValidator,
			[NotNull] IUserPrincipal userPrincipal)
		{
			if (queryRepository == null) throw new ArgumentNullException(nameof(queryRepository));
			if (userAuthorityValidator == null) throw new ArgumentNullException(nameof(userAuthorityValidator));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_queryRepository = queryRepository;
			_userAuthorityValidator = userAuthorityValidator;
			_userPrincipal = userPrincipal;
		}

		public TableDto Execute(GetQueriesListQuery dataQuery)
		{
			if (dataQuery == null)
				throw new ArgumentNullException(nameof(dataQuery));

			var projectsAsAdmin = _userAuthorityValidator.GetProjects(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Queries.ViewQueriesAll
				}).ToArray();

			var projectsAsViewer = _userAuthorityValidator.GetProjects(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Queries.ViewQuery
				}).ToArray();

			var queryIds = new List<long>();

			foreach (var project in projectsAsAdmin)
			{
				var projectQueryIds = _queryRepository.Get(project)
					.Select(_ => _.Id)
					.ToArray();

				queryIds.AddRange(projectQueryIds);
			}

			foreach (var project in projectsAsViewer)
			{
				var projectQueryIds = _queryRepository
					.Get(_userPrincipal.Info.Id, project)
					.Select(_ => _.Id)
					.ToArray();

				queryIds.AddRange(projectQueryIds);
			}

			queryIds = queryIds.Distinct().ToList();

			var queries = _queryRepository.Get(queryIds)
				.Select(_ => new QueryResultItem
				{
					EntityId = _.Id,
					Value = new
					{
						Project = _.Projects.DisplayName,
						_.Name,
						CreatedBy = _.Users.DisplayName,
						ModifiedBy = _.Users1.DisplayName,
						Privacy = ((QueryPrivacyType) _.Privacy).ToString()
					}
				});

			return new TableRenderer().Render(queries);
		}
	}
}