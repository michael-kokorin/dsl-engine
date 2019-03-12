namespace Infrastructure.DataSource
{
	internal interface IDataSourceAuthorityNameBuilder
	{
		string GetDataSourceAuthorityName(string tableName);
	}
}