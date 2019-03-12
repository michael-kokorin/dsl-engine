namespace Infrastructure.DataSource
{
	using System.Collections.Generic;

	public  interface IDataSourceAuthorityProvider
	{
		IEnumerable<string> Get();
	}
}