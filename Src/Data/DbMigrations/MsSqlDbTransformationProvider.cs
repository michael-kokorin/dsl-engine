namespace DbMigrations
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Text.RegularExpressions;
	using System.Threading;

	using Common.Extensions;
	using Common.Logging;
	using Common.Settings;
	using Common.Time;
	using DbUpdateCommon.Extensions;

	internal sealed class MsSqlDbTransformationProvider: DbTransformationProvider
	{
		public MsSqlDbTransformationProvider(
			IConfigManager configurationProvider,
			ILog logger,
			ITimeService timeService)
			: base(configurationProvider, logger, timeService)
		{
			RegisterColumnType(DbType.Int64, "bigint");
			RegisterColumnType(DbType.Binary, 4000, "varbinary($1)");
			RegisterColumnType(DbType.Binary, DbConstants.VarBinaryMax, "varbinary(max)");
			RegisterColumnType(DbType.Boolean, "bit");

			RegisterColumnType(DbType.AnsiStringFixedLength, "char(255)");
			RegisterColumnType(DbType.AnsiStringFixedLength, 8000, "char($l)");
			RegisterColumnType(DbType.AnsiString, "varchar(255)");
			RegisterColumnType(DbType.AnsiString, 8000, "varchar($l)");
			RegisterColumnType(DbType.AnsiString, DbConstants.VarCharMax, "varchar(MAX)");
			RegisterColumnType(DbType.StringFixedLength, "nchar(255)");
			RegisterColumnType(DbType.StringFixedLength, 4000, "nchar($l)");
			RegisterColumnType(DbType.String, "nvarchar(255)");
			RegisterColumnType(DbType.String, 4000, "nvarchar($l)");
			RegisterColumnType(DbType.String, DbConstants.NVarCharMax, "nvarchar(MAX)");
			RegisterColumnType(DbType.Date, "date");
			RegisterColumnType(DbType.DateTime, "datetime");
			RegisterColumnType(DbType.DateTime2, "datetime2");
			RegisterColumnType(DbType.DateTimeOffset, "datetimeoffset");
			RegisterColumnType(DbType.Decimal, "decimal(19,5)");
			RegisterColumnType(DbType.Decimal, 19, "decimal(18, $l)");
			RegisterColumnType(DbType.Double, "float");
			RegisterColumnType(DbType.Int32, "int");
			RegisterColumnType(DbType.Single, "real");
			RegisterColumnType(DbType.Int16, "smallint");
			RegisterColumnType(DbType.Time, "time");
			RegisterColumnType(DbType.Guid, "uniqueidentifier");
			RegisterColumnType(DbType.Byte, "tinyint");
			RegisterColumnType(DbType.Currency, "decimal(16,4)");
			RegisterColumnType(DbType.Xml, "xml");

			RegisterProperty(ColumnProperty.Identity, "identity");
		}

		protected override void AddColumnName(List<string> vals, Column column) => vals.Add($"[{column.Name}]");

		protected override void AddColumnType(List<string> vals, Column column) => vals.Add(GetTypeName(column.ColumnType));

		protected override void AddSqlForIdentityWhichNeedsType(List<string> vals, Column column)
			=> AddValueIfSelected(column, ColumnProperty.Identity, vals);

		protected override void AddSqlForIdentityWhichNotNeedsType(List<string> vals, Column column)
		{
		}

		public override void CreateDatabase()
		{
			var newConnectionString = new Regex("initial catalog=[^;]+").Replace(
				Connection.ConnectionString,
				"initial catalog=master");

			var newConnection = new SqlConnection(newConnectionString);
			try
			{
				newConnection.Open();
				var command = newConnection.CreateCommand();
				command.CommandText = $"CREATE DATABASE {Connection.Database.QuoteIfNeeded()}";
				command.CommandType = CommandType.Text;
				command.ExecuteNonQuery();

				Thread.Sleep(new TimeSpan(0, 0, 0, 5));
			}
			catch (Exception exception)
			{
				Logger.Fatal("Can't create database", exception);
				throw new InvalidOperationException("Can't create database", exception);
			}
			finally
			{
				try
				{
					newConnection.Close();
				}
				catch (Exception exception)
				{
					Logger.Fatal(exception.Format());
					throw;
				}
			}
		}

		public override void CreateSchema(string schemaName)
			=> ExecuteNonQuery($@"CREATE SCHEMA [{schemaName}] AUTHORIZATION[dbo];");
	}
}