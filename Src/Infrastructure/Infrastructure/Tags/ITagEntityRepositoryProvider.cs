namespace Infrastructure.Tags
{
	using Repository;

	public interface ITagEntityRepositoryProvider
	{
		IReadRepository<T> Get<T>() where T : class, IEntity;
	}
}