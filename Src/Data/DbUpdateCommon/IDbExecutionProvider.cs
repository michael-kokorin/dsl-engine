namespace DbUpdateCommon
{
	using System.Data;

	/// <summary>
	///   Provides methods to execution queries over database.
	/// </summary>
	public interface IDbExecutionProvider
	{
		/// <summary>
		///   Begins the transaction.
		/// </summary>
		void BeginTransaction();

		/// <summary>
		/// Opens the connection without transaction.
		/// </summary>
		void OpenConnectionIfClosed();

		/// <summary>
		///   Commits changes..
		/// </summary>
		void Commit();

		/// <summary>
		/// Closes the connection without transaction commit.
		/// </summary>
		void CloseConnectionIfOpen();

		/// <summary>
		///   Executes non-query.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns>Number of rows affected.</returns>
		int ExecuteNonQuery(string sql);

		/// <summary>
		///   Executes the query.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns>The query result reader.</returns>
		IDataReader ExecuteQuery(string sql);

		/// <summary>
		///   Executes the scalar query.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns>The result.</returns>
		object ExecuteScalar(string sql);

		/// <summary>
		///   Gets the full name of the table.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>The full name of the table.</returns>
		string GetFullTableName(string schemaName, string tableName);

		/// <summary>
		///   Rollbacks changes..
		/// </summary>
		void Rollback();
	}
}