namespace Infrastructure.Tags
{
	using Repository.Context;

	public interface ITagProvider
	{
		Tags Add(string tagName);

		Tags Get(string tagName);

		void Remove(Tags tag);
	}
}