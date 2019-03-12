namespace Infrastructure.DataSource
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.Extensions;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class DataSourceFieldInfoProvider : IDataSourceFieldInfoProvider
	{
		private readonly IDataSourceFieldAccessValidator _dataSourceFieldAccessValidator;

		private readonly ITableColumnsRepository _tableColumnsRepository;

		public DataSourceFieldInfoProvider(
			[NotNull] IDataSourceFieldAccessValidator dataSourceFieldAccessValidator,
			[NotNull] ITableColumnsRepository tableColumnsRepository)
		{
			if(dataSourceFieldAccessValidator == null)
				throw new ArgumentNullException(nameof(dataSourceFieldAccessValidator));
			if(tableColumnsRepository == null)
				throw new ArgumentNullException(nameof(tableColumnsRepository));

			_dataSourceFieldAccessValidator = dataSourceFieldAccessValidator;
			_tableColumnsRepository = tableColumnsRepository;
		}

		public IEnumerable<DataSourceFieldInfo> GetBySource(long dataSourceId, long userId)
		{
			var fields = _tableColumnsRepository.GetAvailableByTable(dataSourceId).AsEnumerable();

			var result = fields
				.Where(_ => _dataSourceFieldAccessValidator.CanReadSourceField(_.Id, userId))
				.Select(_ => _.ToFieldInfo())
				.OrderBy(_ => _.Name);

			return result;
		}

		public IEnumerable<DataSourceFieldInfo> GetBySource(string dataSourceKey, long userId)
		{
			if(dataSourceKey == null)
				throw new ArgumentNullException(nameof(dataSourceKey));

			var fields = _tableColumnsRepository.GetAvailableByTable(dataSourceKey).AsEnumerable();

			var result = fields
				.Where(_ => _dataSourceFieldAccessValidator.CanReadSourceField(_.Id, userId))
				.Select(_ => _.ToFieldInfo())
				.OrderBy(_ => _.Name);

			return result;
		}

		private DataSourceFieldInfo Get(string dataSourceKey, string fieldKey, long userId)
		{
			if (string.IsNullOrEmpty(dataSourceKey))
				throw new ArgumentNullException(nameof(dataSourceKey));

			if (string.IsNullOrEmpty(fieldKey))
				throw new ArgumentNullException(nameof(fieldKey));

			var field = GetFieldInfo(dataSourceKey, fieldKey);

			if (field == null)
				throw new DataSourceFieldDoesNotExistsException(dataSourceKey, fieldKey);

			var isCanReadField = _dataSourceFieldAccessValidator.CanReadSourceField(field, userId);

			if (!isCanReadField)
				throw new UnauthorizedAccessException();

			return field.ToFieldInfo();
		}

		private TableColumns GetFieldInfo(string dataSourceKey, string fieldKey) =>
			_tableColumnsRepository.Get(dataSourceKey, fieldKey).SingleOrDefault();

		public DataSourceFieldInfo TryGet(string dataSourceKey, string fieldKey, long userId)
		{
			try
			{
				return Get(dataSourceKey, fieldKey, userId);
			}
			catch
			{
				return null;
			}
		}
	}
}