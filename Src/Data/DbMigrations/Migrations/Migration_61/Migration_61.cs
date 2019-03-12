namespace DbMigrations.Migrations.Migration_61
{
	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_61 : DbMigration
	{
		public override void Up(IDbTransformationProvider database)
		{
			database.ExecuteNonQuery("DELETE FROM [data].[Queries] WHERE [Name]=N'Ftp tech report: project'");
			database.ExecuteNonQuery("DELETE FROM [data].[Queries] WHERE [Name]=N'Ftp tech report: project tasks'");
		}
	}
}