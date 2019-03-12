namespace DbMigrations.Migrations.Migration_56
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	// ReSharper disable once InconsistentNaming
	[UsedImplicitly]
	internal sealed class MigrationPackageMap_56 : MigrationPackagesMap
	{
		public override IEnumerable<KeyValuePair<string, Version>> Packages =>
			new[]
			{
				new KeyValuePair<string, Version>("DataSourceFields", new Version(1, 0, 0, 6))
			};
	}
}