namespace DbUpdateCommon
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;

	using Common.Extensions;
	using Common.Logging;
	using Common.Settings;
	using Common.Time;
	using DbUpdateCommon.Extensions;
	using DbUpdateCommon.Properties;

	/// <summary>
	///   Provides methods to work with database.
	/// </summary>
	/// <seealso cref="DbUpdateCommon.IDbInformationProvider"/>
	public class DbInformationProvider : IDbInformationProvider
	{
		private IDbTransaction _transaction;

		/// <summary>
		///   Initializes a new instance of the <see cref="DbInformationProvider"/> class.
		/// </summary>
		/// <param name="configManager">The application configuration provider.</param>
		/// <param name="logger">The logger.</param>
		/// <param name="timeService">The time service.</param>
		public DbInformationProvider(IConfigManager configManager,
			ILog logger,
			ITimeService timeService)
		{
			ConfigurationProvider = configManager;
			Logger = logger;
			TimeService = timeService;
			InitializeConnection();
		}

		/// <summary>
		///   Gets the configuration provider.
		/// </summary>
		/// <value>
		///   The configuration provider.
		/// </value>
		protected IConfigManager ConfigurationProvider { get; }

		/// <summary>
		///   Gets the connection.
		/// </summary>
		/// <value>
		///   The connection.
		/// </value>
		protected IDbConnection Connection { get; private set; }

		/// <summary>
		///   Gets the logger.
		/// </summary>
		/// <value>
		///   The logger.
		/// </value>
		protected ILog Logger { get; }

		/// <summary>
		///   Gets the time service.
		/// </summary>
		/// <value>
		///   The time service.
		/// </value>
		protected ITimeService TimeService { get; }

		/// <summary>
		/// Closes the connection without transaction commit.
		/// </summary>
		public void CloseConnectionIfOpen()
		{
			if (Connection.State == ConnectionState.Open)
				Connection.Close();
		}

		/// <summary>
		///   Executes non-query.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns>Number of rows affected.</returns>
		public int ExecuteNonQuery(string sql)
		{
			int num;

			Logger.Trace(Resources.ExecutingQuery.FormatWith(sql));

			var dbCommand = BuildCommand(sql);
			try
			{
				num = dbCommand.ExecuteNonQuery();
			}
			catch (Exception exception)
			{
				Logger.Fatal(Resources.QueryFailed.FormatWith(sql), exception);
				throw;
			}

			return num;
		}

		/// <summary>
		///   Executes the query.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns>
		///   The query result reader.
		/// </returns>
		public IDataReader ExecuteQuery(string sql)
		{
			IDataReader dataReader;

			Logger.Trace(Resources.ExecutingQuery.FormatWith(sql));

			var dbCommand = BuildCommand(sql);
			try
			{
				dataReader = dbCommand.ExecuteReader();
			}
			catch (Exception exception)
			{
				Logger.Fatal(Resources.QueryFailed.FormatWith(sql), exception);
				throw;
			}

			return dataReader;
		}

		/// <summary>
		///   Checks that column exists.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns><see langword="true"/> if the column exists; otherwise, <see langword="false"/>.</returns>
		public bool ColumnExists(string schemaName, string tableName, string columnName)
		{
			bool flag;
			try
			{
				var reader = ExecuteQuery(GetSelectColumnLimit1Sql(schemaName, tableName, columnName));
				reader.Close();
				flag = true;
			}
			catch
			{
				flag = false;
			}

			return flag;
		}

		/// <summary>
		///   Begins the transaction.
		/// </summary>
		public void BeginTransaction()
		{
			if ((_transaction != null) || (Connection == null)) return;

			EnsureHasConnection();
			_transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
		}

		/// <summary>
		/// Opens the connection without transaction.
		/// </summary>
		public void OpenConnectionIfClosed()
		{
			if (Connection.State != ConnectionState.Open)
				Connection.Open();
		}

		/// <summary>
		///   Deletes all data from the specified table.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>Number of rows affected.</returns>
		public int Delete(string schemaName, string tableName) => Delete(schemaName, tableName, null, (string[]) null);

		/// <summary>
		///   Truncates the specified table.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>Number of rows affected.</returns>
		public int Truncate(string schemaName, string tableName)
			=> ExecuteNonQuery($"TRUNCATE TABLE {GetFullTableName(schemaName, tableName)}");

		/// <summary>
		///   Deletes data from the table that satisfy the condition.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="whereCondition">The where condition.</param>
		/// <returns>Number of rows affected.</returns>
		public int Delete(string schemaName, string tableName, string whereCondition)
			=>
				ExecuteNonQuery(
					$"DELETE FROM {GetFullTableName(schemaName, tableName)} {(string.IsNullOrWhiteSpace(whereCondition) ? string.Empty : $"WHERE {whereCondition}")}");

		/// <summary>
		///   Deletes data from the table that satisfy the condition.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="whereColumn">The where column.</param>
		/// <param name="whereValue">The where value.</param>
		/// <returns>Number of rows affected.</returns>
		public int Delete(string schemaName, string tableName, string whereColumn, string whereValue)
			=> Delete(schemaName, tableName, $"{whereColumn} = {whereValue}");

		/// <summary>
		///   Deletes data from the table that satisfy the condition.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="whereColumns">The where columns.</param>
		/// <param name="whereValues">The where values.</param>
		/// <returns>Number of rows affected.</returns>
		public int Delete(string schemaName, string tableName, string[] whereColumns, string[] whereValues)
			=> Delete(schemaName, tableName, JoinColumnsAndValues(whereColumns, whereValues, "AND"));

		/// <summary>
		/// Gets the name of the database.
		/// </summary>
		/// <returns>Database name</returns>
		public string GetDatabaseName() => Connection.Database;

		/// <summary>
		///   Executes the scalar query.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns>The result.</returns>
		public object ExecuteScalar(string sql)
		{
			object obj;

			Logger.Trace(Resources.ExecutingQuery.FormatWith(sql));

			var dbCommand = BuildCommand(sql);
			try
			{
				obj = dbCommand.ExecuteScalar();
			}
			catch (Exception exception)
			{
				Logger.Fatal(Resources.QueryFailed.FormatWith(sql), exception);
				throw;
			}
			return obj;
		}

		/// <summary>
		///   Adds the package.
		/// </summary>
		/// <param name="version">The version.</param>
		/// <param name="packageName">Name of the package.</param>
		public void AddPackage(Version version, string packageName) => Insert(
			GetFullTableName(Settings.Default.SystemSchemaName, Settings.Default.PackageVersionTableName),
			new[]
			{
				Settings.Default.ModuleColumnName,
				Settings.Default.VersionColumnName,
				Settings.Default.InstalledColumnName
			},
			new[]
			{
				packageName,
				version.ToString(),
				TimeService.GetUtc().ToString("yyyy-MM-dd hh:mm:ss.fffffff")
			});

		/// <summary>
		///   Checks that the index exists.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="indexName">Name of the index.</param>
		/// <returns><see langword="true"/> if the index exists; otherwise, <see langword="false"/>.</returns>
		public bool IndexExists(string schemaName, string tableName, string indexName)
			=>
				(int)
					ExecuteScalar(
						$@"SELECT COUNT(*) FROM sys.indexes
WHERE name = '{indexName}' AND object_id = OBJECT_ID('{
							GetFullTableName(schemaName, tableName)}')") != 0;

		/// <summary>
		///   The that the constraint exists.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="constraintName">Name of the constraint.</param>
		/// <returns><see langword="true"/> if the constraint exists; otherwise, <see langword="false"/>.</returns>
		public bool ConstraintExists(string schemaName, string tableName, string constraintName) => (int)
			ExecuteScalar(
				$@"SELECT count(*)
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
		WHERE CONSTRAINT_NAME ='{constraintName}'") >
		                                                                                            0;

		/// <summary>
		///   Checks that the schema exists.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <returns><see langword="true"/> if the schema exists; otherwise, <see langword="false"/>.</returns>
		public bool SchemaExists(string schemaName)
			=>
				(int) ExecuteScalar($@"SELECT count(*)
FROM    information_schema.schemata
WHERE   schema_name = '{schemaName}'") !=
				0;

		/// <summary>
		///   Commits changes..
		/// </summary>
		public void Commit()
		{
			if ((_transaction == null) || (Connection == null) || (Connection.State != ConnectionState.Open))
			{
				_transaction = null;
				return;
			}

			try
			{
				_transaction.Commit();
			}
			finally
			{
				Connection.Close();
			}

			_transaction = null;
		}

		/// <summary>
		///   Checks that the table exists.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <returns><see langword="true"/> if the table exists; otherwise, <see langword="false"/>.</returns>
		public virtual bool TableExists(string schemaName, string tableName)
		{
			bool flag;
			try
			{
				ExecuteNonQuery($"SELECT COUNT(*) FROM {GetFullTableName(schemaName, tableName)}");
				flag = true;
			}
			catch
			{
				flag = false;
			}

			return flag;
		}

		/// <summary>
		///   Inserts data into the specified table.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <param name="columns">The columns.</param>
		/// <param name="values">The values.</param>
		/// <returns>Number of rows affected.</returns>
		public virtual int Insert(string table, IEnumerable<string> columns, string[] values) =>
			Insert(table, columns, new[] {values});

		/// <summary>
		///   Inserts data into the specified table.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <param name="columns">The columns.</param>
		/// <param name="values">The values.</param>
		/// <returns>Number of rows affected.</returns>
		public virtual int Insert(string table, IEnumerable<string> columns, IEnumerable<string[]> values) =>
			ExecuteNonQuery(
				$"INSERT INTO {table} ({string.Join(", ", columns.Select(_ => _.QuoteIfNeeded()))}) VALUES {string.Join(",", values.Select(_ => $"({string.Join(", ", _.Quote())})"))}");

		/// <summary>
		///   Rollbacks changes..
		/// </summary>
		public virtual void Rollback()
		{
			if ((_transaction != null) && (Connection != null) && (Connection.State == ConnectionState.Open))
			{
				try
				{
					_transaction.Rollback();
				}
				finally
				{
					Connection.Close();
				}
			}
			_transaction = null;
		}

		/// <summary>
		///   Gets the database part version.
		/// </summary>
		/// <param name="dbPartName">Name of the database part.</param>
		/// <returns>The database part version.</returns>
		public virtual int GetDbPartVersion(string dbPartName)
		{
			EnsureHasConnection();

			int result;

			try
			{
				result = (int) ExecuteScalar(GetSelectVersionQuery(dbPartName));
			}
			catch (Exception exception)
			{
				Logger.Warning(exception.Message, exception);
				result = 0;
			}

			return result;
		}

		/// <summary>
		///   Gets the package versions.
		/// </summary>
		/// <param name="packageName">Name of the package.</param>
		/// <returns>The package versions.</returns>
		public Version[] GetPackageVersions(string packageName)
		{
			EnsureHasConnection();

			var result = ExecuteQuery(
				$"SELECT [{Settings.Default.VersionColumnName}] from {GetFullTableName(Settings.Default.SystemSchemaName, Settings.Default.PackageVersionTableName)} WHERE [{Settings.Default.ModuleColumnName}] ='{packageName}'");

			using (result)
			{
				var output = new List<Version>();
				while (result.Read())
				{
					var version = result.GetString(0);
					output.Add(new Version(version));
				}

				return output.ToArray();
			}
		}

		/// <summary>
		///   Gets the full name of the table.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>The full name of the table.</returns>
		public string GetFullTableName(string schemaName, string tableName) => $"[{schemaName}].[{tableName}]";

		private IDbCommand BuildCommand(string sql)
		{
			var dbCommand = Connection.CreateCommand();
			dbCommand.CommandText = sql;
			dbCommand.CommandType = CommandType.Text;
			if (_transaction != null)
				dbCommand.Transaction = _transaction;

			return dbCommand;
		}

		/// <summary>
		///   Deletes the module.
		/// </summary>
		/// <param name="moduleName">Name of the module.</param>
		public void DeleteModule(string moduleName) => ExecuteNonQuery(
			$"DELETE FROM {GetFullTableName(Settings.Default.SystemSchemaName, Settings.Default.PackageVersionTableName)} WHERE {Settings.Default.ModuleColumnName.QuoteIfNeeded()} = '{moduleName}'");

		/// <summary>
		///   Ensures the provider has connection.
		/// </summary>
		protected virtual void EnsureHasConnection()
		{
			if (Connection.State == ConnectionState.Open) return;

			try
			{
				Connection.Open();
			}
			catch (Exception exception)
			{
				Logger.Warning(exception.Format());

				throw;
			}
		}

		/// <summary>
		///   Gets the select column SQL with limit 1.
		/// </summary>
		/// <param name="schemaName">Name of the schema.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns>The SQL for command.</returns>
		protected string GetSelectColumnLimit1Sql(string schemaName, string tableName, string columnName)
			=> $"SELECT TOP 1 {columnName.QuoteIfNeeded()} FROM {GetFullTableName(schemaName, tableName)}";

		/// <summary>
		///   Gets the select version query.
		/// </summary>
		/// <param name="moduleName">Name of the module.</param>
		/// <returns>The SQL for command.</returns>
		public string GetSelectVersionQuery(string moduleName) =>
			$"select top 1 [{Settings.Default.VersionColumnName}] from {GetFullTableName(Settings.Default.SystemSchemaName, Settings.Default.DbVersionTableName)} where [{Settings.Default.ModuleColumnName}] = '{moduleName}'";

		/// <summary>
		///   Initializes the connection.
		/// </summary>
		protected void InitializeConnection() =>
			Connection = new SqlConnection
			{
				ConnectionString = ConfigurationManager.ConnectionStrings["SdlDbConnection"].ConnectionString
			};

		/// <summary>
		///   Joins the columns and values.
		/// </summary>
		/// <param name="columns">The columns.</param>
		/// <param name="values">The values.</param>
		/// <param name="separator">The separator.</param>
		/// <returns>String containing joined keys and values.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="separator"/> is <see langword="null"/> or empty.</exception>
		public string JoinColumnsAndValues(IEnumerable<string> columns, string[] values, string separator)
		{
			if (string.IsNullOrEmpty(separator))
			{
				Logger.Fatal(Resources.SeparatorIsNotSpecified);
				throw new ArgumentNullException(nameof(separator), Resources.SeparatorIsNotSpecified);
			}

			separator = " " + separator.Trim() + " ";

			var strArrays = values.Quote();
			var array = columns.Select((str, i) => $"{str}={strArrays[i]}").ToArray();
			return string.Join(separator, array);
		}
	}
}