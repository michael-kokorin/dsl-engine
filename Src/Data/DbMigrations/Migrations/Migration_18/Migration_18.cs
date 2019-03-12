namespace DbMigrations.Migrations.Migration_18
{
	using JetBrains.Annotations;

	using Common.FileSystem;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_18: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.ExecuteNonQuery(FileLoader.FromResource($"{GetType().Namespace}.DeleteWrongPermissions.sql"));

			database.ExecuteNonQuery(FileLoader.FromResource($"{GetType().Namespace}.UpdatePolicyRules.sql"));
		}
	}
}