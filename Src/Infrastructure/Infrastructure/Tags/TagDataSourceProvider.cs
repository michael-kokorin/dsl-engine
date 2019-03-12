namespace Infrastructure.Tags
{
	using JetBrains.Annotations;

	using Common.Security;
	using Infrastructure.DataSource;
	using Repository;

	[UsedImplicitly]
	internal sealed class TagDataSourceProvider : ITagDataSourceProvider
	{
		private readonly IDataSourceInfoProvider _dataSourceInfoProvider;

		private readonly IUserPrincipal _userPrincipal;

		public TagDataSourceProvider(IDataSourceInfoProvider dataSourceInfoProvider, IUserPrincipal userPrincipal)
		{
			_dataSourceInfoProvider = dataSourceInfoProvider;
			_userPrincipal = userPrincipal;
		}

		public DataSourceInfo Get<T>(T entity) where T : class, IEntity
		{
			var dataSource = _dataSourceInfoProvider.Get(GetTypeName<T>(), _userPrincipal.Info.Id);

			return dataSource;
		}

		public DataSourceInfo Get<T>(long? projectId)
		{
			var dataSource = _dataSourceInfoProvider.Get(GetTypeName<T>(), _userPrincipal.Info.Id);

			return dataSource;
		}

		private static string GetTypeName<T>() => typeof(T).Namespace;
	}
}