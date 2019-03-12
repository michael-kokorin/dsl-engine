namespace Common.Data
{
	using System;

	/// <summary>
	///   Provides methods to get data source.
	/// </summary>
	public interface IDataSourceProvider
	{
		/// <summary>
		///   Gets data source for the specified type of entity.
		/// </summary>
		/// <typeparam name="T">The type of entity.</typeparam>
		/// <returns>Data source.</returns>
		IDataSource<T> GetDataSource<T>();

		/// <summary>
		///   Gets data source for the specified type of entity.
		/// </summary>
		/// <param name="entityType">Type of the entity.</param>
		/// <returns>
		///   Data source.
		/// </returns>
		IDataSource<object> GetDataSource(Type entityType);
	}
}