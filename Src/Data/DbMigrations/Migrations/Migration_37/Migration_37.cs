namespace DbMigrations.Migrations.Migration_37
{
	using JetBrains.Annotations;

	using Common.FileSystem;
	using DbUpdateCommon;

	[UsedImplicitly]
	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_37 : DbMigration
	{
		/// <summary>
		/// Is transaction required for migration
		/// </summary>
		public override bool RequireTransaction => false;

		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database) => ExecuteSqlFile(database, "AllowSnapshotIsolation");

		private void ExecuteSqlFile(IDbInformationProvider database, string fileName)
		{
			var dbName = database.GetDatabaseName();

			var allowSnapshotIsolation = FileLoader.FromResource($"{GetType().Namespace}.{fileName}.sql")
				.Replace("dbName", dbName);

			database.ExecuteNonQuery(allowSnapshotIsolation);
		}
	}
}