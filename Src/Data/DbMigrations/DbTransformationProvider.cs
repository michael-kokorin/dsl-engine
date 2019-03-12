namespace DbMigrations
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;

	using Common.Extensions;
	using Common.Logging;
	using Common.Settings;
	using Common.Time;
	using DbMigrations.Properties;
	using DbUpdateCommon;
	using DbUpdateCommon.Extensions;
	using DbUpdateCommon.Properties;

	internal abstract class DbTransformationProvider: DbInformationProvider, IDbTransformationProvider
	{
		private readonly ForeignKeyConstraintMapper _constraintMapper = new ForeignKeyConstraintMapper();

		private readonly Dictionary<ColumnProperty, string> _propertyMap = new Dictionary<ColumnProperty, string>();

		private readonly TypeNames _typeNames = new TypeNames();

		protected DbTransformationProvider(
			IConfigManager configManager,
			ILog logger,
			ITimeService timeService)
			: base(configManager, logger, timeService)
		{
			RegisterProperty(ColumnProperty.Null, "NULL");
			RegisterProperty(ColumnProperty.NotNull, "NOT NULL");
			RegisterProperty(ColumnProperty.Unique, "UNIQUE");
			RegisterProperty(ColumnProperty.PrimaryKey, "PRIMARY KEY CLUSTERED");
		}

		/// <summary>
		///     Changes the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="sqlColumn">The SQL column.</param>
		public void ChangeColumn(string schemaName, string tableName, string sqlColumn) =>
			ExecuteNonQuery($"ALTER TABLE {GetFullTableName(schemaName, tableName)} ALTER COLUMN {sqlColumn}");

		/// <summary>
		///     Changes the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		public void ChangeColumn(string schemaName, string tableName, Column column)
		{
			if (!ColumnExists(schemaName, tableName, column.Name))
			{
				var message = Resources.ColumnDoesntExist.FormatWith(column.Name, GetFullTableName(schemaName, tableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			var changeColumnSql = GetChangeColumnSql(column);
			ChangeColumn(schemaName, tableName, changeColumnSql);
		}

		/// <summary>
		///     Migrations the applied.
		/// </summary>
		/// <param name="version">The version.</param>
		/// <param name="dbPartName">Name of the database part.</param>
		public virtual void MigrationApplied(int version, string dbPartName)
		{
			EnsureHasConnection();

			var result = ExecuteNonQuery(GetUpdateVersionSql(version, dbPartName));
			if (result != 0) return;

			Insert(
				GetFullTableName(Settings.Default.SystemSchemaName, Settings.Default.DbVersionTableName),
				new[]
				{
					Settings.Default.VersionColumnName,
					Settings.Default.ModuleColumnName
				},
				new[]
				{
					version.ToString(),
					dbPartName
				});
		}

		/// <summary>
		///     Sets the database part version.
		/// </summary>
		/// <param name="dbPartName">Name of the database part.</param>
		/// <param name="version">The version.</param>
		public void SetDbPartVersion(string dbPartName, int version)
		{
			var result =
				ExecuteNonQuery(
					$"UPDATE {GetFullTableName(Settings.Default.SystemSchemaName, Settings.Default.DbVersionTableName)} SET [{Settings.Default.VersionColumnName}] = {version} where [{Settings.Default.ModuleColumnName}] = '{dbPartName}'");
			if (result == 0)
			{
				Insert(
					GetFullTableName(Settings.Default.SystemSchemaName, Settings.Default.DbVersionTableName),
					new[]
					{
						Settings.Default.VersionColumnName,
						Settings.Default.ModuleColumnName
					},
					new[]
					{
						version.ToString(),
						dbPartName
					});
			}
		}

		/// <summary>
		///     Adds the unique constraint.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		/// <exception cref="InvalidOperationException">Constraint already exists.</exception>
		public void AddUniqueConstraint(string schemaName, string tableName, params string[] columns)
		{
			var constraintName = GetUniqueConstraintName(schemaName, tableName, columns);
			if (ConstraintExists(schemaName, tableName, constraintName))
			{
				var message = Resources.ConstraintAlreadyExists.FormatWith(constraintName, GetFullTableName(schemaName, tableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			ExecuteNonQuery(
				$"ALTER TABLE {GetFullTableName(schemaName, tableName)} ADD CONSTRAINT {constraintName.QuoteIfNeeded()} UNIQUE({(from col in columns select col.QuoteIfNeeded()).ToCommaSeparatedString()}) ");
		}

		/// <summary>
		///     Creates the schema.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		public abstract void CreateSchema(string schemaName);

		/// <summary>
		///     Adds the table.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		public virtual void AddTable(string schemaName, string tableName, string columns)
		{
			EnsureSchemaExist(schemaName);

			var str = $"CREATE TABLE {GetFullTableName(schemaName, tableName)} ({columns})";

			ExecuteNonQuery(str);
		}

		/// <summary>
		///     Adds the table.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		/// <exception cref="InvalidOperationException">Table already exists.</exception>
		public virtual void AddTable(string schemaName, string tableName, params Column[] columns)
		{
			if (TableExists(schemaName, tableName))
			{
				var message = Resources.TableAlreadyExists.FormatWith(GetFullTableName(schemaName, tableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			var primaryKeys = GetPrimaryKeys(columns);
			if (primaryKeys.Count == 0)
			{
				var message = Resources.TableDoesntContainPrimaryKey.FormatWith(GetFullTableName(schemaName, tableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			var strs = new List<string>(columns.Length);
			var columnArray = columns;

			foreach (var column in columnArray)
			{
				if (column.IsPrimaryKey)
					column.ColumnProperty = (column.ColumnProperty | ColumnProperty.NotNull) ^ ColumnProperty.PrimaryKey;

				strs.Add(GetColumnSql(column, primaryKeys.Count > 1));
			}

			AddTable(schemaName, tableName, strs.ToCommaSeparatedString());
			AddPrimaryKey(schemaName, tableName, primaryKeys.ToArray());
		}

		/// <summary>
		///     Adds the table with identifier column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		public void AddTableWithIdColumn(string schemaName, string tableName, params Column[] columns)
		{
			columns = new[]
			{
				new Column("Id", DbType.Int64, ColumnProperty.Identity)
			}.Concat(columns).ToArray();
			AddTable(schemaName, tableName, columns);
		}

		/// <summary>
		///     Adds the table with identifier primary key column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		public void AddTableWithIdPrimaryKeyColumn(string schemaName, string tableName, params Column[] columns)
		{
			columns = new[]
			{
				new Column("Id", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity)
			}.Concat(columns).ToArray();
			AddTable(schemaName, tableName, columns);
		}

		/// <summary>
		///     Adds the check constraint.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="constraintName">Name of the constraint.</param>
		/// <param name="checkSql">The check SQL.</param>
		/// <exception cref="InvalidOperationException"></exception>
		public void AddCheckConstraint(string schemaName, string tableName, string constraintName, string checkSql)
		{
			if (ConstraintExists(schemaName, tableName, constraintName))
			{
				var message = Resources.ConstraintAlreadyExists.FormatWith(constraintName, GetFullTableName(schemaName, tableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			ExecuteNonQuery(
				$"ALTER TABLE {GetFullTableName(schemaName, tableName)} ADD CONSTRAINT {constraintName} CHECK ({checkSql}) ");
		}

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="sqlColumn">The SQL column.</param>
		public void AddColumn(string schemaName, string tableName, string sqlColumn)
			=> ExecuteNonQuery($"ALTER TABLE {GetFullTableName(schemaName, tableName)} ADD {sqlColumn}");

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
		public void AddColumn(
				string schemaName,
				string tableName,
				string columnName,
				DbType type,
				int size,
				ColumnProperty property,
				object defaultValue)
			=> AddColumn(schemaName, tableName, new Column(columnName, type, size, property, defaultValue));

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		/// <exception cref="InvalidOperationException"></exception>
		public void AddColumn(string schemaName, string tableName, Column column)
		{
			if (ColumnExists(schemaName, tableName, column.Name))
			{
				var message = Resources.ColumnAlreadyExists.FormatWith(column.Name, GetFullTableName(schemaName, tableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			AddColumn(schemaName, tableName, GetColumnSql(column, false));
		}

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		/// <param name="type">The type.</param>
		public void AddColumn(string schemaName, string tableName, string column, DbType type) =>
			AddColumn(schemaName, tableName, column, type, 0, ColumnProperty.Null, null);

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="type">The type.</param>
		/// <param name="property">The property.</param>
		/// <param name="defaultValue">The default value.</param>
		public void AddColumn(
				string schemaName,
				string tableName,
				string columnName,
				ColumnType type,
				ColumnProperty property,
				object defaultValue)
			=> AddColumn(schemaName, tableName, new Column(columnName, type, property, defaultValue));

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		/// <param name="type">The type.</param>
		/// <param name="size">The size.</param>
		public void AddColumn(string schemaName, string tableName, string column, DbType type, int size)
			=> AddColumn(schemaName, tableName, column, type, size, ColumnProperty.Null, null);

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		/// <param name="type">The type.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <exception cref="InvalidOperationException">Column already exists.</exception>
		public void AddColumn(string schemaName, string tableName, string column, DbType type, object defaultValue)
		{
			if (!ColumnExists(schemaName, tableName, column))
			{
				var message = Resources.ColumnAlreadyExists.FormatWith(column, GetFullTableName(schemaName, tableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			AddColumn(schemaName, tableName, new Column(column, type, defaultValue));
		}

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		/// <param name="type">The type.</param>
		/// <param name="property">The property.</param>
		public void AddColumn(string schemaName, string tableName, string column, DbType type, ColumnProperty property)
			=> AddColumn(schemaName, tableName, column, type, 0, property, null);

		/// <summary>
		///     Adds the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="column">The column.</param>
		/// <param name="type">The type.</param>
		/// <param name="size">The size.</param>
		/// <param name="property">The property.</param>
		public void AddColumn(
			string schemaName,
			string tableName,
			string column,
			DbType type,
			int size,
			ColumnProperty property) => AddColumn(schemaName, tableName, column, type, size, property, null);

		/// <summary>
		///     Adds the foreign key.
		/// </summary>
		/// <param name="primaryTableSchemaName">Name of the primary table schema.</param>
		/// <param name="primaryTableName">Name of the primary table.</param>
		/// <param name="primaryColumns">The primary columns.</param>
		/// <param name="refTableSchemaName">Name of the reference table schema.</param>
		/// <param name="refTabelName">Name of the reference tabel.</param>
		/// <param name="refColumns">The reference columns.</param>
		public void AddForeignKey(
				string primaryTableSchemaName,
				string primaryTableName,
				string[] primaryColumns,
				string refTableSchemaName,
				string refTabelName,
				string[] refColumns)
			=>
			AddForeignKey(
				primaryTableSchemaName,
				primaryTableName,
				primaryColumns,
				refTableSchemaName,
				refTabelName,
				refColumns,
				ForeignKeyConstraint.NoAction);

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
		public void AddForeignKey(
				string primaryTableSchemaName,
				string primaryTableName,
				string primaryColumn,
				string refTableSchemaName,
				string refTabelName,
				string refColumn,
				ForeignKeyConstraint constraint)
			=>
			AddForeignKey(
				primaryTableSchemaName,
				primaryTableName,
				new[]
				{
					primaryColumn
				},
				refTableSchemaName,
				refTabelName,
				new[]
				{
					refColumn
				},
				constraint);

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
		public void AddForeignKey(
				string primaryTableSchemaName,
				string primaryTableName,
				string[] primaryColumns,
				string refTableSchemaName,
				string refTabelName,
				string[] refColumns,
				ForeignKeyConstraint constraint)
			=>
			AddForeignKey(
				primaryTableSchemaName,
				primaryTableName,
				primaryColumns,
				refTableSchemaName,
				refTabelName,
				refColumns,
				constraint,
				ForeignKeyConstraint.NoAction);

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
		/// <exception cref="InvalidOperationException">Foreign key already exists.</exception>
		public void AddForeignKey(
			string primaryTableSchemaName,
			string primaryTableName,
			string[] primaryColumns,
			string refTableSchemaName,
			string refTabelName,
			string[] refColumns,
			ForeignKeyConstraint onDeleteConstraint,
			ForeignKeyConstraint onUpdateConstraint)
		{
			var keyName = GetForeignKeyConstraintName(
				primaryTableSchemaName,
				primaryTableName,
				primaryColumns,
				refTableSchemaName,
				refTabelName,
				refColumns);

			if (ConstraintExists(primaryTableSchemaName, primaryTableName, keyName))
			{
				var message = Resources.ConstraintAlreadyExists.FormatWith(
					keyName,
					GetFullTableName(primaryTableSchemaName, primaryTableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			var query =
				$"ALTER TABLE {GetFullTableName(primaryTableSchemaName, primaryTableName)} ADD CONSTRAINT {keyName} FOREIGN KEY ({string.Join(",", primaryColumns)}) REFERENCES {GetFullTableName(refTableSchemaName, refTabelName)} ({string.Join(",", refColumns)}) ON UPDATE {ForeignKeyConstraintMapper.SqlForConstraint(onUpdateConstraint)} ON DELETE {ForeignKeyConstraintMapper.SqlForConstraint(onDeleteConstraint)}";

			ExecuteNonQuery(query);
		}

		/// <summary>
		///     Adds the index.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="unique">if set to <see langword="true"/> [unique].</param>
		/// <param name="columns">The columns.</param>
		/// <exception cref="InvalidOperationException">Index already exists.</exception>
		public void AddIndex(string schemaName, string tableName, bool unique, params string[] columns)
		{
			var indexName = GetIndexConstraintName(schemaName, tableName, unique, columns);
			if (columns.Length == 0)
			{
				Logger.Fatal(Resources.EmptyColumnsForIndex);
				throw new InvalidOperationException(Resources.EmptyColumnsForIndex);
			}

			if (IndexExists(schemaName, tableName, indexName))
			{
				var message = Resources.IndexAlreadyExists.FormatWith(indexName, GetFullTableName(schemaName, tableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			ExecuteNonQuery(
				$"CREATE {(unique ? "UNIQUE" : string.Empty)} INDEX {indexName.QuoteIfNeeded()} ON {GetFullTableName(schemaName, tableName)} ({columns.Select(_ => _.QuoteIfNeeded()).ToCommaSeparatedString()})");
		}

		/// <summary>
		///     Adds the primary key.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		/// <exception cref="InvalidOperationException">Primary key already exists.</exception>
		public virtual void AddPrimaryKey(string schemaName, string tableName, params string[] columns)
		{
			var constraintName = GetPrimaryKeyConstraintName(schemaName, tableName, columns);
			if (ConstraintExists(schemaName, tableName, constraintName))
			{
				var message = Resources.PrimaryKeyAlreadyExists.FormatWith(constraintName);
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			string str =
				$"ALTER TABLE {GetFullTableName(schemaName, tableName)} ADD CONSTRAINT {constraintName.QuoteIfNeeded()} PRIMARY KEY ({(from col in columns select col.QuoteIfNeeded()).ToCommaSeparatedString()}) ";
			ExecuteNonQuery(str);
		}

		/// <summary>
		///     Removes the column.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <exception cref="InvalidOperationException">Column doesn't exist.</exception>
		public void RemoveColumn(string schemaName, string tableName, string columnName)
		{
			if (!ColumnExists(schemaName, tableName, columnName))
			{
				var message = Resources.ColumnDoesntExist.FormatWith(columnName, GetFullTableName(schemaName, tableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			ExecuteNonQuery($"ALTER TABLE {GetFullTableName(schemaName, tableName)} DROP COLUMN {columnName.QuoteIfNeeded()}");
		}

		/// <summary>
		///     Removes the constraint.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="constraintName">Name of the constraint.</param>
		/// <exception cref="InvalidOperationException">Constraint doesn't exist.</exception>
		public void RemoveConstraint(string schemaName, string tableName, string constraintName)
		{
			if (!TableExists(schemaName, tableName))
			{
				var message = Resources.TableDoesntExist.FormatWith(GetFullTableName(schemaName, tableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			if (!ConstraintExists(schemaName, tableName, constraintName))
			{
				var message = Resources.ConstraintDoesntExist.FormatWith(constraintName, GetFullTableName(schemaName, tableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			ExecuteNonQuery(
				$"ALTER TABLE {GetFullTableName(schemaName, tableName)} DROP CONSTRAINT {constraintName.QuoteIfNeeded()}");
		}

		/// <summary>
		///     Removes the index.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="indexName">Name of the index.</param>
		/// <exception cref="InvalidOperationException">Index doesn't exist.</exception>
		public void RemoveIndex(string schemaName, string tableName, string indexName)
		{
			if (!IndexExists(schemaName, tableName, indexName))
			{
				var message = Resources.IndexDoesntExist.FormatWith(indexName, GetFullTableName(schemaName, tableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			ExecuteNonQuery($"DROP INDEX {indexName.QuoteIfNeeded()} ON {GetFullTableName(schemaName, tableName)}");
		}

		/// <summary>
		///     Removes the table.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <exception cref="InvalidOperationException">Tables doesn't exist.</exception>
		public void RemoveTable(string schemaName, string tableName)
		{
			if (!TableExists(schemaName, tableName))
			{
				var message = Resources.TableDoesntExist.FormatWith(GetFullTableName(schemaName, tableName));
				Logger.Fatal(message);
				throw new InvalidOperationException(message);
			}

			ExecuteNonQuery($"DROP TABLE {GetFullTableName(schemaName, tableName)}");
		}

		/// <summary>
		///     Adds the name of the column.
		/// </summary>
		/// <param name="vals">The vals.</param>
		/// <param name="column">The column.</param>
		protected abstract void AddColumnName(List<string> vals, Column column);

		/// <summary>
		///     Adds the type of the column.
		/// </summary>
		/// <param name="vals">The vals.</param>
		/// <param name="column">The column.</param>
		protected abstract void AddColumnType(List<string> vals, Column column);

		/// <summary>
		///     Adds the default value SQL.
		/// </summary>
		/// <param name="vals">The vals.</param>
		/// <param name="column">The column.</param>
		protected void AddDefaultValueSql(List<string> vals, Column column)
		{
			if (column.DefaultValue != null)
				vals.Add(Default(column.DefaultValue));
		}

		/// <summary>
		///     Adds the foreign key SQL.
		/// </summary>
		/// <param name="vals">The vals.</param>
		/// <param name="column">The column.</param>
		protected void AddForeignKeySql(List<string> vals, Column column)
			=> AddValueIfSelected(column, ColumnProperty.ForeignKey, vals);

		/// <summary>
		///     Adds the not null SQL.
		/// </summary>
		/// <param name="vals">The vals.</param>
		/// <param name="column">The column.</param>
		protected void AddNotNullSql(List<string> vals, Column column)
			=> AddValueIfSelected(column, ColumnProperty.NotNull, vals);

		protected void AddNotOrNullSql(List<string> vals, Column column)
		{
			if (column.ColumnProperty.HasProperty(ColumnProperty.NotNull))
				AddValueIfSelected(column, ColumnProperty.NotNull, vals);
			else
				vals.Add(SqlForProperty(ColumnProperty.Null));
		}

		/// <summary>
		///     Adds the primary key SQL.
		/// </summary>
		/// <param name="vals">The vals.</param>
		/// <param name="column">The column.</param>
		/// <param name="compoundPrimaryKey">if set to <see langword="true"/> [compound primary key].</param>
		protected void AddPrimaryKeySql(List<string> vals, Column column, bool compoundPrimaryKey)
		{
			if (!compoundPrimaryKey)
				AddValueIfSelected(column, ColumnProperty.PrimaryKey, vals);
		}

		/// <summary>
		///     Adds the type of the SQL for identity which needs.
		/// </summary>
		/// <param name="vals">The vals.</param>
		/// <param name="column">The column.</param>
		protected abstract void AddSqlForIdentityWhichNeedsType(List<string> vals, Column column);

		/// <summary>
		///     Adds the type of the SQL for identity which not needs.
		/// </summary>
		/// <param name="vals">The vals.</param>
		/// <param name="column">The column.</param>
		protected abstract void AddSqlForIdentityWhichNotNeedsType(List<string> vals, Column column);

		/// <summary>
		///     Adds the unique SQL.
		/// </summary>
		/// <param name="vals">The vals.</param>
		/// <param name="column">The column.</param>
		protected void AddUniqueSql(List<string> vals, Column column)
			=> AddValueIfSelected(column, ColumnProperty.Unique, vals);

		/// <summary>
		///     Adds the unsigned SQL.
		/// </summary>
		/// <param name="vals">The vals.</param>
		/// <param name="column">The column.</param>
		protected void AddUnsignedSql(List<string> vals, Column column)
			=> AddValueIfSelected(column, ColumnProperty.Unsigned, vals);

		/// <summary>
		///     Adds the value if selected.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <param name="property">The property.</param>
		/// <param name="vals">The vals.</param>
		protected void AddValueIfSelected(Column column, ColumnProperty property, ICollection<string> vals)
		{
			if (column.ColumnProperty.HasProperty(property))
				vals.Add(SqlForProperty(property));
		}

		/// <summary>
		///     Builds the column SQL.
		/// </summary>
		/// <param name="vals">The vals.</param>
		/// <param name="column">The column.</param>
		/// <param name="compoundPrimaryKey">if set to <see langword="true"/> the compound primary key is used.</param>
		protected void BuildColumnSql(List<string> vals, Column column, bool compoundPrimaryKey)
		{
			AddColumnName(vals, column);
			AddColumnType(vals, column);
			AddSqlForIdentityWhichNotNeedsType(vals, column);
			AddUnsignedSql(vals, column);
			AddNotNullSql(vals, column);
			AddPrimaryKeySql(vals, column, compoundPrimaryKey);
			AddSqlForIdentityWhichNeedsType(vals, column);
			AddUniqueSql(vals, column);
			AddForeignKeySql(vals, column);
			AddDefaultValueSql(vals, column);
		}

		/// <summary>
		///     Creates the database.
		/// </summary>
		public abstract void CreateDatabase();

		public string Default(object defaultValue) => $"DEFAULT {defaultValue}";

		/// <summary>
		///     Ensures the provider has connection.
		/// </summary>
		protected override void EnsureHasConnection()
		{
			try
			{
				base.EnsureHasConnection();
			}
			catch
			{
				try
				{
					CreateDatabase();
					InitializeConnection();
					base.EnsureHasConnection();
				}
				catch (Exception innerException)
				{
					Logger.Fatal(innerException.Format());
					throw;
				}
			}
		}

		/// <summary>
		///     Ensures the schema exist.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		public void EnsureSchemaExist(string schemaName)
		{
			if (SchemaExists(schemaName))
				return;

			CreateSchema(schemaName);
		}

		public string GetChangeColumnSql(Column column)
		{
			var vals = new List<string>();

			AddColumnName(vals, column);
			AddColumnType(vals, column);
			AddSqlForIdentityWhichNotNeedsType(vals, column);
			AddUnsignedSql(vals, column);
			AddNotOrNullSql(vals, column);
			AddSqlForIdentityWhichNeedsType(vals, column);
			AddUniqueSql(vals, column);
			AddForeignKeySql(vals, column);
			AddDefaultValueSql(vals, column);

			return string.Join(" ", vals);
		}

		/// <summary>
		///     Gets the column SQL.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <param name="compoundPrimaryKey">if set to <see langword="true"/> compound primary key is used.</param>
		/// <returns>The column SQL.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="column"/> is <see langword="null"/>.</exception>
		public string GetColumnSql(Column column, bool compoundPrimaryKey)
		{
			if (column == null)
				throw new ArgumentNullException(nameof(column));

			var strs = new List<string>();
			BuildColumnSql(strs, column, compoundPrimaryKey);
			return string.Join(" ", strs.ToArray());
		}

		/// <summary>
		///     Gets the name of the foreign key constraint.
		/// </summary>
		/// <param name="primaryTableSchemaName">Name of the primary table schema.</param>
		/// <param name="primaryTableName">Name of the primary table.</param>
		/// <param name="primaryColumns">The primary columns.</param>
		/// <param name="refTableSchemaName">Name of the reference table schema.</param>
		/// <param name="refTableName">Name of the reference table.</param>
		/// <param name="refColumns">The reference columns.</param>
		/// <returns>The name of the foreign key constraint.</returns>
		public string GetForeignKeyConstraintName(
				string primaryTableSchemaName,
				string primaryTableName,
				string[] primaryColumns,
				string refTableSchemaName,
				string refTableName,
				string[] refColumns) =>
			$"FK_{primaryTableName}_{refTableName}_{string.Join("_", primaryColumns)}";

		/// <summary>
		///     Gets the name of the index constraint.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="unique">if set to <see langword="true"/> [unique].</param>
		/// <param name="columns">The columns.</param>
		/// <returns>The name of the index constraint.</returns>
		public string GetIndexConstraintName(string schemaName, string tableName, bool unique, string[] columns) =>
			$"IX_{tableName}_{string.Join("_", columns)}";

		/// <summary>
		///     Gets the name of the primary key constraint.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		/// <returns>The name of the primary key constraint.</returns>
		public string GetPrimaryKeyConstraintName(string schemaName, string tableName, string[] columns) =>
			$"PK_{tableName}";

		/// <summary>
		///     Gets the primary keys.
		/// </summary>
		/// <param name="columns">The columns.</param>
		/// <returns>The primary keys</returns>
		public static List<string> GetPrimaryKeys(IEnumerable<Column> columns) => (
			from column in columns
			where column.IsPrimaryKey
			select column.Name).ToList();

		/// <summary>
		///     Gets the name of the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The name of the type</returns>
		public string GetTypeName(ColumnType type) => _typeNames.Get(type);

		/// <summary>
		///     Gets the name of the unique constraint.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columns">The columns.</param>
		/// <returns>The name of the unique constraint.</returns>
		public string GetUniqueConstraintName(string schemaName, string tableName, string[] columns) =>
			$"UK_{tableName}_{string.Join("_", columns)}";

		public string GetUpdateVersionSql(int version, string moduleName)
			=>
			$"UPDATE {GetFullTableName(Settings.Default.SystemSchemaName, Settings.Default.DbVersionTableName)} SET [{Settings.Default.VersionColumnName}] = {version} where [{Settings.Default.ModuleColumnName}] = '{moduleName}'";

		/// <summary>
		///     Registers the type of the column.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="capacity">The capacity.</param>
		/// <param name="name">The name.</param>
		protected void RegisterColumnType(DbType code, int capacity, string name) => _typeNames.Put(code, capacity, name);

		/// <summary>
		///     Registers the type of the column.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="capacity">The capacity.</param>
		/// <param name="name">The name.</param>
		/// <param name="defaultScale">The default scale.</param>
		protected void RegisterColumnType(DbType code, int capacity, string name, int defaultScale)
			=> _typeNames.Put(code, capacity, name, defaultScale);

		/// <summary>
		///     Registers the type of the column.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="name">The name.</param>
		protected void RegisterColumnType(DbType code, string name) => _typeNames.Put(code, name);

		/// <summary>
		///     Registers the property.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <param name="sql">The SQL.</param>
		public void RegisterProperty(ColumnProperty property, string sql)
		{
			if (!_propertyMap.ContainsKey(property))
				_propertyMap.Add(property, sql);

			_propertyMap[property] = sql;
		}

		/// <summary>
		///     Get SQL for property.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns>SQL for property.</returns>
		public string SqlForProperty(ColumnProperty property)
			=> !_propertyMap.ContainsKey(property) ? string.Empty : _propertyMap[property];
	}
}