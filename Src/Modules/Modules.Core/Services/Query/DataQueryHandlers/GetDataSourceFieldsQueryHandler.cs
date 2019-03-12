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
	internal sealed class GetDataSourceFieldsQueryHandler : IDataQueryHandler<GetDataSourceFieldsQuery, DataSourceFieldDto[]>
	{
		private readonly IDataSourceFieldInfoProvider _dataSourceFieldInfoProvider;

		private readonly IUserPrincipal _userPrincipal;

		public GetDataSourceFieldsQueryHandler([NotNull] IDataSourceFieldInfoProvider dataSourceFieldInfoProvider,
			[NotNull] IUserPrincipal userPrincipal)
		{
			if (dataSourceFieldInfoProvider == null) throw new ArgumentNullException(nameof(dataSourceFieldInfoProvider));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_dataSourceFieldInfoProvider = dataSourceFieldInfoProvider;
			_userPrincipal = userPrincipal;
		}

		public DataSourceFieldDto[] Execute(GetDataSourceFieldsQuery getDataQuery)
		{
			if (getDataQuery == null)
				throw new ArgumentNullException(nameof(getDataQuery));

			var fields = _dataSourceFieldInfoProvider.GetBySource(getDataQuery.DataSourceKey, _userPrincipal.Info.Id);

			return fields.Select(_ => new DataSourceFieldDto
			{
				Description = _.Description,
				Id = _.Id,
				Key = _.Key,
				Name = _.Name,
				ReferencedDataSourceId = _.ReferenceTableId
			})
			.OrderBy(_ => _.Name)
			.ToArray();
		}
	}
}