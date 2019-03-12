namespace Infrastructure.Query
{
	using Repository.Context;

	public interface IQueryAccessValidator
	{
		bool IsCanEdit(Queries query, long userId);

		bool IsCanView(Queries query, long userId);
	}
}