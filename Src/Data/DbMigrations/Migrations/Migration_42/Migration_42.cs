using JetBrains.Annotations;

using Common.FileSystem;

namespace DbMigrations.Migrations.Migration_42
{
    [UsedImplicitly]
    // ReSharper disable once InconsistentNaming
    internal sealed class Migration_42 : DbMigration
    {
        /// <summary>
        ///   Performs migration.
        /// </summary>
        public override void Up(IDbTransformationProvider database) =>
            database.ExecuteNonQuery(FileLoader.FromResource($"{GetType().Namespace}.l10n.GetSettings.sql"));
    }
}