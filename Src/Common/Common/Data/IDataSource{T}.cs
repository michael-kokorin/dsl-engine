namespace Common.Data
{
	using System.Linq;

	/// <summary>
	///   Contract to mark any data source.
	/// </summary>
	public interface IDataSource
	{
		/// <summary>
		///   Queries data.
		/// </summary>
		/// <returns>Data.</returns>
		IQueryable<object> Query();
	}

	/// <summary>
	///   Represents contract to any data source.
	/// </summary>
	/// <typeparam name="T">Type of entity of source.</typeparam>
	public interface IDataSource<out T>: IDataSource
	{
		/// <summary>
		///   Queries data.
		/// </summary>
		/// <returns>Data.</returns>
		new IQueryable<T> Query();
	}
}