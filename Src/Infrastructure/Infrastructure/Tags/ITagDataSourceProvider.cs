namespace Infrastructure.Tags
{
	using Infrastructure.DataSource;
	using Repository;

	public interface ITagDataSourceProvider
	{
		DataSourceInfo Get<T>(T entity) where T : class, IEntity;

		DataSourceInfo Get<T>(long? projectId);
	}
}