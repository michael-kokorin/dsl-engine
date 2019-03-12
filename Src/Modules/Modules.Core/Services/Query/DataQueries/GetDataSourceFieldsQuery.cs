namespace Modules.Core.Services.Query.DataQueries
{
	using System;

	using Common.Query;

	internal sealed class GetDataSourceFieldsQuery : IDataQuery
	{
		public readonly string DataSourceKey;

		public readonly long? ProjectId;

		public GetDataSourceFieldsQuery(string dataSourceKey, long? projectId)
		{
			if (string.IsNullOrEmpty(dataSourceKey))
				throw new ArgumentNullException(nameof(dataSourceKey));

			DataSourceKey = dataSourceKey;

			ProjectId = projectId;
		}
	}
}