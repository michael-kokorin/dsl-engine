namespace Infrastructure.DataSource
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	// ReSharper disable once MemberCanBeInternal
	public interface IDataSourceAccessValidator
	{
		bool CanReadSource(long dataSourceId, long userId);

		bool CanReadSource([NotNull] string dataSourceKey, long userId);

		IEnumerable<long> GetDataSourceProjects([NotNull] string dataSourceKey, long userId);
	}
}