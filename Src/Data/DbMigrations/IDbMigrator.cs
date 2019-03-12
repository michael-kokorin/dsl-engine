namespace DbMigrations
{
	/// <summary>
	///   Provides methods to perform database migration.
	/// </summary>
	public interface IDbMigrator
	{
		/// <summary>
		///   Migrations the database to the latest version.
		/// </summary>
		void MigrationLatest();
	}
}