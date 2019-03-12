namespace Modules.Core.Services.Query.DataQueryHandlers
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Query;
	using Common.Security;
	using Infrastructure.DataSource;
	using Modules.Core.Contracts.Query.Dto;
	using Modules.Core.Services.Query.DataQueries;

	[UsedImplicitly]
	internal sealed class DataSourcesQueryHandler : IDataQueryHandler<DataSourcesQuery, DataSourceDto[]>
	{
		private readonly IDataSourceInfoProvider _dataSourceInfoProvider;

		private readonly IUserPrincipal _userPrincipal;

		public DataSourcesQueryHandler([NotNull] IDataSourceInfoProvider dataSourceInfoProvider,
			[NotNull] IUserPrincipal userPrincipal)
		{
			if (dataSourceInfoProvider == null) throw new ArgumentNullException(nameof(dataSourceInfoProvider));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_dataSourceInfoProvider = dataSourceInfoProvider;
			_userPrincipal = userPrincipal;
		}

		public DataSourceDto[] Execute(DataSourcesQuery dataQuery)
		{
			if (dataQuery == null)
				throw new ArgumentNullException(nameof(dataQuery));

			var dataSources = _dataSourceInfoProvider.Get(_userPrincipal.Info.Id);

			return dataSources.Select(_ => new DataSourceDto
			{
				Description = _.Description,
				Id = _.Id,
				Key = _.Key,
				Name = _.Name
			})
			.OrderBy(_ => _.Name)
			.ToArray();
		}
	}
}