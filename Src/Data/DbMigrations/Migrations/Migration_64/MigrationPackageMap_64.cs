namespace DbMigrations.Migrations.Migration_64
{
	using System;
	using System.Collections.Generic;

	// ReSharper disable once InconsistentNaming
	internal sealed class MigrationPackageMap_64 : MigrationPackagesMap
	{
		public override IEnumerable<KeyValuePair<string, Version>> Packages =>
			new[]
			{
				new KeyValuePair<string, Version>("Reports", new Version(1, 0, 0, 10))
			};
	}
}