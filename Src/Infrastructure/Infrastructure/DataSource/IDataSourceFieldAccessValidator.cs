namespace Infrastructure.DataSource
{
	using Repository.Context;

	public interface IDataSourceFieldAccessValidator
	{
		bool CanReadSourceField(long fieldId, long userId);

		bool CanReadSourceField(TableColumns column, long userId);
	}
}