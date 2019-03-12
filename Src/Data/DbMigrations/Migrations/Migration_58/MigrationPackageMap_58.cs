namespace DbMigrations.Migrations.Migration_58
{
	using System;
	using System.Collections.Generic;

	// ReSharper disable once InconsistentNaming
	internal sealed class MigrationPackageMap_58 : MigrationPackagesMap
	{
		public override IEnumerable<KeyValuePair<string, Version>> Packages =>
			new[]
			{
				new KeyValuePair<string, Version>("Queries", new Version(1, 0, 0, 7)),
				new KeyValuePair<string, Version>("DataSourceFields", new Version(1, 0, 0, 6))
			};
	}
}