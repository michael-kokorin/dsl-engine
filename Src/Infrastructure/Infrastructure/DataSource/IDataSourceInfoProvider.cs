namespace Infrastructure.DataSource
{
	using System.Collections.Generic;

	public interface IDataSourceInfoProvider
	{
		DataSourceInfo Get(string dataSourceName, long userId);

		IEnumerable<DataSourceInfo> Get(long userId);
	}
}