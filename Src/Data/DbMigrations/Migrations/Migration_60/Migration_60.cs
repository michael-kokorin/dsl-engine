namespace DbMigrations.Migrations.Migration_60
{
	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_60 : DbMigration
	{
		public override void Up(IDbTransformationProvider database)
		{
			database.ExecuteNonQuery("DELETE FROM [data].[Queries] WHERE [Name]=N'Scan results report results'");
			database.ExecuteNonQuery("DELETE FROM [data].[Queries] WHERE [Name]=N'Scan results report task'");

			database.ExecuteNonQuery("DELETE FROM [data].[Queries] WHERE [Name]=N'Project Scan Core Settings'");
			database.ExecuteNonQuery("DELETE FROM [data].[Queries] WHERE [Name]=N'Task Scan Core Settings'");
		}
	}
}