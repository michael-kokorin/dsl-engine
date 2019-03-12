namespace Infrastructure.Extensions
{
	using Infrastructure.DataSource;
	using Repository.Context;

	internal static class DatabaseExtension
	{
		public static DataSourceFieldInfo ToFieldInfo(this TableColumns column) =>
			new DataSourceFieldInfo
			{
				DataSourceId = column.TableId,
				DataType = column.FieldDataType,
				Description = column.FieldDescription,
				Id = column.Id,
				Key = column.Name,
				Name = column.FieldName,
				ReferenceTableId = column.ReferenceTableId
			};
	}
}