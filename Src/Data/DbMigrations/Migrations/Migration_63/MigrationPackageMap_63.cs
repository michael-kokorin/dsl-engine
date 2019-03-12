namespace DbMigrations.Migrations.Migration_63
{
	using System;
	using System.Collections.Generic;

	// ReSharper disable once InconsistentNaming
	internal sealed class MigrationPackageMap_63 : MigrationPackagesMap
	{
		public override IEnumerable<KeyValuePair<string, Version>> Packages =>
			new[]
			{
				new KeyValuePair<string, Version>("DataSourceFields", new Version(1, 0, 0, 8))
			};
	}
}