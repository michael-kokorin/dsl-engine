namespace Infrastructure.DataSource
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class DataSourceAuthorityProvider : IDataSourceAuthorityProvider
	{
		private readonly IDataSourceAuthorityNameBuilder _dataSourceAuthorityNameBuilder;

		private readonly ITableRepository _tableRepository;

		public DataSourceAuthorityProvider(
			[NotNull] IDataSourceAuthorityNameBuilder dataSourceAuthorityNameBuilder,
			[NotNull] ITableRepository tableRepository)
		{
			if (dataSourceAuthorityNameBuilder == null) throw new ArgumentNullException(nameof(dataSourceAuthorityNameBuilder));
			if (tableRepository == null) throw new ArgumentNullException(nameof(tableRepository));

			_dataSourceAuthorityNameBuilder = dataSourceAuthorityNameBuilder;
			_tableRepository = tableRepository;
		}

		public IEnumerable<string> Get()
		{
			var tableNames = _tableRepository
				.GetAvailable()
				.Select(_ => _.Name)
				.ToArray();

			return tableNames.Select(_ => _dataSourceAuthorityNameBuilder.GetDataSourceAuthorityName(_));
		}
	}
}