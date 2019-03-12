namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage table columns.
	/// </summary>
	public interface ITableColumnsRepository: ILocalizedRepository<TableColumns>
	{
		/// <summary>
		///   Gets table columns by the specified table name and field name.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>Columns.</returns>
		IQueryable<TableColumns> Get(string tableName, string fieldName);

		/// <summary>
		///   Gets all available table columns.
		/// </summary>
		/// <returns>Columns.</returns>
		IQueryable<TableColumns> GetAvailable();

		/// <summary>
		///   Gets available table columns with the specified identifier.
		/// </summary>
		/// <param name="tableColumnId">The table column identifier.</param>
		/// <returns>Columns.</returns>
		IQueryable<TableColumns> GetAvailable(long tableColumnId);

		/// <summary>
		///   Gets available columns by table.
		/// </summary>
		/// <param name="tableId">The table identifier.</param>
		/// <returns></returns>
		IQueryable<TableColumns> GetAvailableByTable(long tableId);

		/// <summary>
		///   Gets available columns by table.
		/// </summary>
		/// <param name="tableKey">The table key.</param>
		/// <returns>Columns.</returns>
		IQueryable<TableColumns> GetAvailableByTable(string tableKey);

		/// <summary>
		///   Gets columns by table.
		/// </summary>
		/// <param name="tableId">The table identifier.</param>
		/// <returns>Columns.</returns>
		IQueryable<TableColumns> GetByTable(long tableId);
	}
}