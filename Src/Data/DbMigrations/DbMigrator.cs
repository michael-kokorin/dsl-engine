namespace DbMigrations
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Logging;
	using DbMigrations.Properties;
	using DbUpdateCommon.Properties;
	using Packages;

	[UsedImplicitly]
	internal sealed class DbMigrator : IDbMigrator
	{
		private readonly IDbTransformationProvider _dbTransformationProvider;

		private readonly IPackageInstaller _installer;

		private readonly ILog _logger;

		private readonly MigrationPackagesMap[] _maps;

		private readonly IPackagesProvider _provider;

		public DbMigrator(
			IDbTransformationProvider dbTransformationProvider,
			IPackageInstaller installer,
			IPackagesProvider provider,
			ILog logger)
		{
			_dbTransformationProvider = dbTransformationProvider;
			_logger = logger;
			_installer = installer;
			_provider = provider;
			_maps = GetPackagesMap();
		}

		/// <summary>
		///   Migrations the database to the latest version.
		/// </summary>
		public void MigrationLatest()
		{
			var currentVersion = _dbTransformationProvider.GetDbPartVersion(Settings.Default.SchemaDbPart);

			var migrations = GetMigrations(currentVersion);

			if (migrations.Length == 0)
			{
				_logger.Trace(Resources.DbIsUpToDate);

				return;
			}

			ApplyMigrations(migrations, currentVersion);
		}

		private void ApplyMigration([NotNull] DbMigration migration, int currentVersion)
		{
			try
			{
				_logger.Debug(Resources.DbMigrator_MigrationApplying.FormatWith(
					migration.GetType().FullName,
					currentVersion,
					migration.Version));

				if (migration.RequireTransaction)
				{
					_dbTransformationProvider.BeginTransaction();
				}
				else
				{
					_dbTransformationProvider.OpenConnectionIfClosed();
				}

				migration.Up(_dbTransformationProvider);

				_dbTransformationProvider.SetDbPartVersion(Settings.Default.SchemaDbPart, migration.Version);

				var item = _maps.FirstOrDefault(x => x.MigrationVersion == migration.Version);

				if ((item?.Packages != null) && item.Packages.Any())
				{
					var migrationPackages = item.Packages.Select(packageInfo => _provider.Get(packageInfo.Key, packageInfo.Value));

					foreach (var package in migrationPackages)
					{
						_installer.Install(_dbTransformationProvider, package);
					}
				}

				if (migration.RequireTransaction)
				{
					_dbTransformationProvider.Commit();
				}
				else
				{
					_dbTransformationProvider.CloseConnectionIfOpen();
				}

				_logger.Debug(
					Resources.MigrationAppliedSuccessfully.FormatWith(
						migration.GetType().FullName,
						currentVersion,
						migration.Version));
			}
			catch (Exception exception)
			{
				if (migration.RequireTransaction)
				{
					_dbTransformationProvider.Rollback();
				}

				var message = Resources.MigrationError.FormatWith(migration.GetType().FullName, currentVersion, migration.Version);
				_logger.Error(message);
				throw new InvalidOperationException(message, exception);
			}
		}

		private void ApplyMigrations([NotNull] [ItemNotNull] DbMigration[] migrations, int? currentVersion)
		{
			if (currentVersion == null)
			{
				currentVersion = 0;
			}

			foreach (var migration in migrations)
			{
				if (currentVersion == migration.Version)
				{
					var message = Resources.MigrationVersionIsDuplicated.FormatWith(
						currentVersion,
						string.Join(", ", migrations.Where(_ => _.Version == currentVersion).Select(_ => _.GetType().FullName)));

					_logger.Fatal(message);

					throw new InvalidOperationException(message);
				}

				ApplyMigration(migration, currentVersion.Value);

				currentVersion = migration.Version;
			}
		}

		[NotNull]
		[ItemNotNull]
		private static DbMigration[] GetMigrations(int dbVersion, int? targetVersion = null)
			=> AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => _.FullName.Contains("DbMigrations"))
				.SelectMany(_ => _.GetTypes())
				.Where(_ => typeof(DbMigration).IsAssignableFrom(_) && (typeof(DbMigration) != _))
				.Select(_ => Activator.CreateInstance(_) as DbMigration)
				.Where(_ => dbVersion < _.Version)
				.WhereIf(targetVersion != null, _ => _.Version <= targetVersion)
				.OrderBy(_ => _.Version)
				.ToArray();

		[NotNull]
		[ItemNotNull]
		private static MigrationPackagesMap[] GetPackagesMap() =>
			AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => _.FullName.Contains("DbMigrations"))
				.SelectMany(_ => _.GetTypes())
				.Where(_ => typeof(MigrationPackagesMap).IsAssignableFrom(_) && (typeof(MigrationPackagesMap) != _))
				.Select(_ => Activator.CreateInstance(_) as MigrationPackagesMap)
				.ToArray();
	}
}