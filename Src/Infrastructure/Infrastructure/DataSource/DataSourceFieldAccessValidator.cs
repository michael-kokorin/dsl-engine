namespace Infrastructure.DataSource
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class DataSourceFieldAccessValidator: IDataSourceFieldAccessValidator
	{
		private readonly IDataSourceAccessValidator _dataSourceAccessValidator;

		private readonly ITableColumnsRepository _tableColumnsRepository;

		public DataSourceFieldAccessValidator(
			[NotNull] IDataSourceAccessValidator dataSourceAccessValidator,
			[NotNull] ITableColumnsRepository tableColumnsRepository)
		{
			if(dataSourceAccessValidator == null)
				throw new ArgumentNullException(nameof(dataSourceAccessValidator));
			if(tableColumnsRepository == null)
				throw new ArgumentNullException(nameof(tableColumnsRepository));

			_dataSourceAccessValidator = dataSourceAccessValidator;
			_tableColumnsRepository = tableColumnsRepository;
		}

		public bool CanReadSourceField(long fieldId, long userId)
		{
			var field = _tableColumnsRepository
				.GetAvailable(fieldId)
				.SingleOrDefault();

			if (field == null)
				throw new ArgumentException(nameof(fieldId));

			return CanReadSourceField(field, userId);
		}

		public bool CanReadSourceField([NotNull] TableColumns column, long userId)
		{
			if (column == null)
				throw new ArgumentNullException(nameof(column));

			var canReadDataSource = _dataSourceAccessValidator.CanReadSource(column.TableId, userId);

			if (!canReadDataSource)
				return false;

			return column.FieldType != (int)DataSourceFieldType.Closed;
		}
	}
}