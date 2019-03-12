namespace Infrastructure.Extensions
{
	using Infrastructure.DataSource;
	using Repository.Context;

	internal static class EntityExtension
	{
		public static DataSourceInfo ToDto(this Tables table) => new DataSourceInfo
		{
			Description = table.DataSourceDescription,
			Id = table.Id,
			Key = table.Name,
			Name = table.DataSourceName
		};
	}
}