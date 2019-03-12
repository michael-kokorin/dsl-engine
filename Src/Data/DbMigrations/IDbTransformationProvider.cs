namespace DbMigrations
{
	using System.Data;

	using DbUpdateCommon;

	/// <summary>
	///     Provides methods to transform schema of database.
	/// </summary>
	/// <seealso cref="DbUpdateCommon.IDbInformationProvider"/>
	internal interface IDbTransformationProvider: IDbInformationProvider
	{
		/// <summary>
		///     Adds the check constraint.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="constraintName">Name of the constraint.</param>
		/// <param name="checkSql">The check SQL.</param>
		void AddCheckConstraint(string schemaName, string tableName, string constraintName, string checkSql);

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="sqlColumn">The SQL column.</param>
		void AddColumn(string schemaName, string tableName, string sqlColumn);

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="type">The type.</param>
		/// <param name="size">The size.</param>
		/// <param name="property">The property.</param>
		/// <param name="defaultValue">The default value.</param>
		void AddColumn(
			string schemaName,
			string tableName,
			string columnName,
			DbType type,
			int size,
			ColumnProperty property,
			object defaultValue);

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		void AddColumn(string schemaName, string tableName, Column column);

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		/// <param name="type">The type.</param>
		void AddColumn(string schemaName, string tableName, string column, DbType type);

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="type">The type.</param>
		/// <param name="property">The property.</param>
		/// <param name="defaultValue">The default value.</param>
		void AddColumn(
			string schemaName,
			string tableName,
			string columnName,
			ColumnType type,
			ColumnProperty property,
			object defaultValue);

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		/// <param name="type">The type.</param>
		/// <param name="size">The size.</param>
		void AddColumn(string schemaName, string tableName, string column, DbType type, int size);

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		/// <param name="type">The type.</param>
		/// <param name="defaultValue">The default value.</param>
		void AddColumn(string schemaName, string tableName, string column, DbType type, object defaultValue);

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		/// <param name="type">The type.</param>
		/// <param name="property">The property.</param>
		void AddColumn(string schemaName, string tableName, string column, DbType type, ColumnProperty property);

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		/// <param name="type">The type.</param>
		/// <param name="size">The size.</param>
		/// <param name="property">The property.</param>
		void AddColumn(string schemaName, string tableName, string column, DbType type, int size, ColumnProperty property);

		/// <summary>
		///     Adds the foreign key.
		/// </summary>
		/// <param name="primaryTableSchemaName">Name of the primary table schema.</param>
		/// <param name="primaryTableName">Name of the primary table.</param>
		/// <param name="primaryColumns">The primary columns.</param>
		/// <param name="refTableSchemaName">Name of the reference table schema.</param>
		/// <param name="refTabelName">Name of the reference tabel.</param>
		/// <param name="refColumns">The reference columns.</param>
		void AddForeignKey(
			string primaryTableSchemaName,
			string primaryTableName,
			string[] primaryColumns,
			string refTableSchemaName,
			string refTabelName,
			string[] refColumns);

		/// <summary>
		///     Adds the foreign key.
		/// </summary>
		/// <param name="primaryTableSchemaName">Name of the primary table schema.</param>
		/// <param name="primaryTableName">Name of the primary table.</param>
		/// <param name="primaryColumn">The primary column.</param>
		/// <param name="refTableSchemaName">Name of the reference table schema.</param>
		/// <param name="refTabelName">Name of the reference tabel.</param>
		/// <param name="refColumn">The reference column.</param>
		/// <param name="constraint">The constraint.</param>
		void AddForeignKey(
			string primaryTableSchemaName,
			string primaryTableName,
			string primaryColumn,
			string refTableSchemaName,
			string refTabelName,
			string refColumn = "Id",
			ForeignKeyConstraint constraint = ForeignKeyConstraint.NoAction);

		/// <summary>
		///     Adds the foreign key.
		/// </summary>
		/// <param name="primaryTableSchemaName">Name of the primary table schema.</param>
		/// <param name="primaryTableName">Name of the primary table.</param>
		/// <param name="primaryColumns">The primary columns.</param>
		/// <param name="refTableSchemaName">Name of the reference table schema.</param>
		/// <param name="refTabelName">Name of the reference tabel.</param>
		/// <param name="refColumns">The reference columns.</param>
		/// <param name="constraint">The constraint.</param>
		void AddForeignKey(
			string primaryTableSchemaName,
			string primaryTableName,
			string[] primaryColumns,
			string refTableSchemaName,
			string refTabelName,
			string[] refColumns,
			ForeignKeyConstraint constraint);

		/// <summary>
		///     Adds the foreign key.
		/// </summary>
		/// <param name="primaryTableSchemaName">Name of the primary table schema.</param>
		/// <param name="primaryTableName">Name of the primary table.</param>
		/// <param name="primaryColumns">The primary columns.</param>
		/// <param name="refTableSchemaName">Name of the reference table schema.</param>
		/// <param name="refTabelName">Name of the reference tabel.</param>
		/// <param name="refColumns">The reference columns.</param>
		/// <param name="onDeleteConstraint">The on delete constraint.</param>
		/// <param name="onUpdateConstraint">The on update constraint.</param>
		void AddForeignKey(
			string primaryTableSchemaName,
			string primaryTableName,
			string[] primaryColumns,
			string refTableSchemaName,
			string refTabelName,
			string[] refColumns,
			ForeignKeyConstraint onDeleteConstraint,
			ForeignKeyConstraint onUpdateConstraint);

		/// <summary>
		///     Adds the index.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="unique">if set to <see langword="true"/> [unique].</param>
		/// <param name="columns">The columns.</param>
		void AddIndex(string schemaName, string tableName, bool unique, params string[] columns);

		/// <summary>
		///     Adds the primary key.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		void AddPrimaryKey(string schemaName, string tableName, params string[] columns);

		/// <summary>
		///     Adds the table.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		void AddTable(string schemaName, string tableName, string columns);

		/// <summary>
		///     Adds the table.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		void AddTable(string schemaName, string tableName, params Column[] columns);

		/// <summary>
		///     Adds the table with identifier column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		void AddTableWithIdColumn(string schemaName, string tableName, params Column[] columns);

		/// <summary>
		///     Adds the table with identifier primary key column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		void AddTableWithIdPrimaryKeyColumn(string schemaName, string tableName, params Column[] columns);

		/// <summary>
		///     Adds the unique constraint.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		void AddUniqueConstraint(string schemaName, string tableName, params string[] columns);

		/// <summary>
		///     Changes the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		void ChangeColumn(string schemaName, string tableName, Column column);

		/// <summary>
		///     Changes the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="sqlColumn">The SQL column.</param>
		void ChangeColumn(string schemaName, string tableName, string sqlColumn);

		/// <summary>
		///     Creates the schema.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		void CreateSchema(string schemaName);

		/// <summary>
		///     Migrations the applied.
		/// </summary>
		/// <param name="version">The version.</param>
		/// <param name="dbPartName">Name of the database part.</param>
		void MigrationApplied(int version, string dbPartName);

		/// <summary>
		///     Removes the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		void RemoveColumn(string schemaName, string tableName, string columnName);

		/// <summary>
		///     Removes the constraint.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="constraintName">Name of the constraint.</param>
		void RemoveConstraint(string schemaName, string tableName, string constraintName);

		/// <summary>
		///     Removes the index.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="indexName">Name of the index.</param>
		void RemoveIndex(string schemaName, string tableName, string indexName);

		/// <summary>
		///     Removes the table.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		void RemoveTable(string schemaName, string tableName);

		/// <summary>
		///     Sets the database part version.
		/// </summary>
		/// <param name="dbPartName">Name of the database part.</param>
		/// <param name="version">The version.</param>
		void SetDbPartVersion(string dbPartName, int version);
	}
}