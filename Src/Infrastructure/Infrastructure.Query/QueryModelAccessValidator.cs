namespace Infrastructure.Query
{
	using System;

	using JetBrains.Annotations;

	using Common.Security;
	using Infrastructure.DataSource;
	using Infrastructure.Engines.Dsl.Query;

	[UsedImplicitly]
	internal sealed class QueryModelAccessValidator : IQueryModelAccessValidator
	{
		private readonly IDataSourceInfoProvider _dataSourceInfoProvider;

		private readonly IDataSourceAccessValidator _dataSourceAccessValidator;

		private readonly IUserPrincipal _userPrincipal;

		public QueryModelAccessValidator([NotNull] IDataSourceInfoProvider dataSourceInfoProvider,
			[NotNull] IDataSourceAccessValidator dataSourceAccessValidator,
			[NotNull] IUserPrincipal userPrincipal)
		{
			if (dataSourceInfoProvider == null) throw new ArgumentNullException(nameof(dataSourceInfoProvider));
			if (dataSourceAccessValidator == null) throw new ArgumentNullException(nameof(dataSourceAccessValidator));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_dataSourceInfoProvider = dataSourceInfoProvider;
			_dataSourceAccessValidator = dataSourceAccessValidator;
			_userPrincipal = userPrincipal;
		}

		public void Validate(DslDataQuery query, long? projectId, bool isSystem)
		{
			if (query == null)
				throw new ArgumentNullException(nameof(query));

			if ((projectId == null) && isSystem)
				return;

			var tableDataSource = _dataSourceInfoProvider.Get(query.QueryEntityName, _userPrincipal.Info.Id);

			var isDataSourceFromTable = tableDataSource != null;

			if (!isDataSourceFromTable) return;

			if (!_dataSourceAccessValidator.CanReadSource(tableDataSource.Id, _userPrincipal.Info.Id))
			{
				throw new UnauthorizedAccessException();
			}
		}
	}
}