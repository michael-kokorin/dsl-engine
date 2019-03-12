namespace Infrastructure.Tags
{
	using Repository;
	using Repository.Context;

	public interface ITagEntityProvider
	{
		void Add<T>(T entity, Tags tag) where T : class, IEntity;

		bool Exists(Tags tag);

		void Remove<T>(T entity, Tags tag) where T : class, IEntity;
	}
}