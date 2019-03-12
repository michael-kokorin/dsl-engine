namespace DbUpdateCommon
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	/// <summary>
	///   Provides methods to interact with database.
	/// </summary>
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	public interface IDbInformationProvider: IDbExecutionProvider
	{
		/// <summary>
		///   Adds the package.
		/// </summary>
		/// <param name="version">The version.</param>
		/// <param name="packageName">Name of the package.</param>
		void AddPackage(Version version, string packageName);

		/// <summary>
		///   Checks that column exists.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns><see langword="true"/> if the column exists; otherwise, <see langword="false"/>.</returns>
		bool ColumnExists(string schemaName, string tableName, string columnName);

		/// <summary>
		///   The that the constraint exists.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="constraintName">Name of the constraint.</param>
		/// <returns><see langword="true"/> if the constraint exists; otherwise, <see langword="false"/>.</returns>
		bool ConstraintExists(string schemaName, string tableName, string constraintName);

		/// <summary>
		///   Deletes all data from the specified table.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>Number of rows affected.</returns>
		int Delete(string schemaName, string tableName);

		/// <summary>
		///   Deletes data from the table that satisfy the condition.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="whereCondition">The where condition.</param>
		/// <returns>Number of rows affected.</returns>
		int Delete(string schemaName, string tableName, string whereCondition);

		/// <summary>
		///   Deletes data from the table that satisfy the condition.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="whereColumn">The where column.</param>
		/// <param name="whereValue">The where value.</param>
		/// <returns>Number of rows affected.</returns>
		int Delete(string schemaName, string tableName, string whereColumn, string whereValue);

		/// <summary>
		///   Deletes data from the table that satisfy the condition.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="whereColumns">The where columns.</param>
		/// <param name="whereValues">The where values.</param>
		/// <returns>Number of rows affected.</returns>
		int Delete(string schemaName, string tableName, string[] whereColumns, string[] whereValues);

		/// <summary>
		/// Gets the name of the database.
		/// </summary>
		/// <returns>Database name</returns>
		string GetDatabaseName();

		/// <summary>
		///   Gets the database part version.
		/// </summary>
		/// <param name="dbPartName">Name of the database part.</param>
		/// <returns>The database part version.</returns>
		int GetDbPartVersion(string dbPartName);

		/// <summary>
		///   Gets the package versions.
		/// </summary>
		/// <param name="packageName">Name of the package.</param>
		/// <returns>The package versions.</returns>
		Version[] GetPackageVersions(string packageName);

		/// <summary>
		///   Checks that the index exists.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="indexName">Name of the index.</param>
		/// <returns><see langword="true"/> if the index exists; otherwise, <see langword="false"/>.</returns>
		bool IndexExists(string schemaName, string tableName, string indexName);

		/// <summary>
		///   Inserts data into the specified table.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <param name="columns">The columns.</param>
		/// <param name="values">The values.</param>
		/// <returns>Number of rows affected.</returns>
		int Insert(string table, IEnumerable<string> columns, string[] values);

		/// <summary>
		///   Inserts data into the specified table.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <param name="columns">The columns.</param>
		/// <param name="values">The values.</param>
		/// <returns>Number of rows affected.</returns>
		int Insert(string table, IEnumerable<string> columns, IEnumerable<string[]> values);

		/// <summary>
		///   Checks that the schema exists.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <returns><see langword="true"/> if the schema exists; otherwise, <see langword="false"/>.</returns>
		bool SchemaExists(string schemaName);

		/// <summary>
		///   Checks that the table exists.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <returns><see langword="true"/> if the table exists; otherwise, <see langword="false"/>.</returns>
		bool TableExists(string schemaName, string tableName);

		/// <summary>
		///   Truncates the specified table.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>Number of rows affected.</returns>
		int Truncate(string schemaName, string tableName);
	}
}