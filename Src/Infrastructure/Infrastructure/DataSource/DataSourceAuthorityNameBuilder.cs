namespace Infrastructure.DataSource
{
	using JetBrains.Annotations;

	using Common.Extensions;

	[UsedImplicitly]
	internal sealed class DataSourceAuthorityNameBuilder: IDataSourceAuthorityNameBuilder
	{
		private const string DataSourceAuthorityKey = "read_datasource_{0}";

		public string GetDataSourceAuthorityName(string tableName) =>
			DataSourceAuthorityKey.FormatWith(tableName.ToLower());
	}
}