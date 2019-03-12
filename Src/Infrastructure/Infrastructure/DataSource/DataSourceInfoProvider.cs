namespace Infrastructure.DataSource
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.Extensions;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class DataSourceInfoProvider : IDataSourceInfoProvider
	{
		private readonly ITableRepository _tableRepository;

		private readonly IDataSourceAccessValidator _dataSourceAccessValidator;

		public DataSourceInfoProvider
			([NotNull] IDataSourceAccessValidator dataSourceAccessValidator,
				[NotNull] ITableRepository tableRepository)
		{
			if (dataSourceAccessValidator == null) throw new ArgumentNullException(nameof(dataSourceAccessValidator));
			if (tableRepository == null) throw new ArgumentNullException(nameof(tableRepository));

			_dataSourceAccessValidator = dataSourceAccessValidator;
			_tableRepository = tableRepository;
		}

		public DataSourceInfo Get(string dataSourceName, long userId)
		{
			if (string.IsNullOrEmpty(nameof(dataSourceName)))
				throw new ArgumentNullException(nameof(dataSourceName));

			var dataSource = _tableRepository.Get(dataSourceName).SingleOrDefault();

			if (dataSource == null)
				throw new DataSourceDoesNotExistsException(dataSourceName);

			var canReadSource = _dataSourceAccessValidator.CanReadSource(dataSourceName, userId);

			if (!canReadSource)
				throw new UnauthorizedAccessException();

			return dataSource.ToDto();
		}

		public IEnumerable<DataSourceInfo> Get(long userId)
		{
			var dataSources = _tableRepository.GetAvailable().AsEnumerable();

			dataSources = dataSources.Where(_ => _dataSourceAccessValidator.CanReadSource(_.Id, userId));

			return dataSources
				.Select(_ => _.ToDto())
				.OrderBy(_ => _.Name);
		}
	}
}