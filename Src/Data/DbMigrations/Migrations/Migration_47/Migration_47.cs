namespace DbMigrations.Migrations.Migration_47
{
	using JetBrains.Annotations;

	using Common.FileSystem;

	[UsedImplicitly]
	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_47 : DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			var query = FileLoader.FromResource($"{GetType().Namespace}.DeleteAuthorityFromRoles.sql");

			database.ExecuteNonQuery(query);
		}
	}
}