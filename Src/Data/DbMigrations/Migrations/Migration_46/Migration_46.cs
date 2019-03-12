namespace DbMigrations.Migrations.Migration_46
{
	using JetBrains.Annotations;

	using Common.FileSystem;

	// ReSharper disable once InconsistentNaming
	[UsedImplicitly]
	internal sealed class Migration_46 : DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			var query = "DELETE FROM [data].[Templates] WHERE [Key] NOT IN ('IssueBody', 'CommitTitle')";

			database.ExecuteNonQuery(query);

			query = FileLoader.FromResource($"{GetType().Namespace}.RemoveUnusedQueries.sql");

			database.ExecuteNonQuery(query);

			query = FileLoader.FromResource($"{GetType().Namespace}.RemoveUnusedNotificationRules.sql");

			database.ExecuteNonQuery(query);
		}
	}
}