namespace DbMigrations.Migrations.Migration_59
{
	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_59 : DbMigration
	{
		public override void Up(IDbTransformationProvider database)
		{
			database.ExecuteNonQuery("DELETE FROM [data].[Queries] WHERE [Name]=N'Notification task info'");
			database.ExecuteNonQuery("DELETE FROM [data].[Queries] WHERE [Name]=N'Notification task result info'");
			database.ExecuteNonQuery("DELETE FROM [data].[Queries] WHERE [Name]=N'Notification task result summary'");
		}
	}
}