namespace Common.Data
{
	using System;

	using Microsoft.Practices.Unity;

	internal sealed class DataSourceProvider: IDataSourceProvider
	{
		private readonly IUnityContainer _container;

		/// <summary>
		///   Initializes a new instance of the <see cref="DataSourceProvider"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public DataSourceProvider(IUnityContainer container)
		{
			_container = container;
		}

		/// <summary>
		///   Gets data source for the specified type of entity.
		/// </summary>
		/// <typeparam name="T">The type of entity.</typeparam>
		/// <returns>Data source.</returns>
		public IDataSource<T> GetDataSource<T>() => _container.Resolve<IDataSource<T>>();

		/// <summary>
		///   Gets data source for the specified type of entity.
		/// </summary>
		/// <param name="entityType">Type of the entity.</param>
		/// <returns>
		///   Data source.
		/// </returns>
		public IDataSource<object> GetDataSource(Type entityType) =>
			_container.Resolve<IDataSource>(entityType?.Name) as IDataSource<object>;
	}
}