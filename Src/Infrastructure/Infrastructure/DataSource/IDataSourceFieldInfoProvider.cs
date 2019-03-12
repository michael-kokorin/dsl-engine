namespace Infrastructure.DataSource
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	public interface IDataSourceFieldInfoProvider
	{
		IEnumerable<DataSourceFieldInfo> GetBySource(long dataSourceId, long userId);

		IEnumerable<DataSourceFieldInfo> GetBySource([NotNull] string dataSourceKey, long userId);

		DataSourceFieldInfo TryGet([NotNull] string dataSourceKey, [NotNull] string fieldKey, long userId);
	}
}