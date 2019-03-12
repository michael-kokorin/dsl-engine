namespace Infrastructure.Tags
{
	using System.Linq;

	using Repository;

	public interface ITagService
	{
		void Add<T>(T entity, string tag) where T : class, IEntity;

		void Remove<T>(T entity, string tagName) where T : class, IEntity;

		IQueryable<T> GetByTag<T>(string tagName, long? projectId) where T : class, IEntity;
	}
}