namespace Modules.Core.Services.Query.DataQueryHandlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Query;
	using Common.Security;
	using Infrastructure.Query;
	using Modules.Core.Services.Query.DataQueries;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class CanEditQueryQueryHandler : IDataQueryHandler<CanEditQueryQuery, bool>
	{
		private readonly IQueryAccessValidator _queryAccessValidator;

		private readonly IQueryRepository _queryRepository;

		private readonly IUserPrincipal _userPrincipal;

		public CanEditQueryQueryHandler(
			[NotNull] IQueryAccessValidator queryAccessValidator,
			[NotNull] IQueryRepository queryRepository,
			[NotNull] IUserPrincipal userPrincipal)
		{
			if (queryAccessValidator == null) throw new ArgumentNullException(nameof(queryAccessValidator));
			if (queryRepository == null) throw new ArgumentNullException(nameof(queryRepository));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_queryAccessValidator = queryAccessValidator;
			_queryRepository = queryRepository;
			_userPrincipal = userPrincipal;
		}

		public bool Execute(CanEditQueryQuery dataQuery)
		{
			if (dataQuery == null)
				throw new ArgumentNullException(nameof(dataQuery));

			var query = _queryRepository.GetById(dataQuery.QueryId);

			return _queryAccessValidator.IsCanEdit(query, _userPrincipal.Info.Id);
		}
	}
}