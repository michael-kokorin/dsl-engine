namespace DbMigrations.Migrations.Migration_54
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class MigrationPackageMap_54: MigrationPackagesMap
	{
		public override IEnumerable<KeyValuePair<string, Version>> Packages =>
			new[]
			{
				new KeyValuePair<string, Version>("DataSource", new Version(1, 0, 0, 4)),
				new KeyValuePair<string, Version>("DataSourceFields", new Version(1, 0, 0, 5))
			};
	}
}