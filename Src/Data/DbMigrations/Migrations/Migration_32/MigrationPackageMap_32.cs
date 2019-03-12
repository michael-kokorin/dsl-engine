namespace DbMigrations.Migrations.Migration_32
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class MigrationPackageMap_32: MigrationPackagesMap
	{
		/// <summary>
		///   Gets the packages.
		/// </summary>
		/// <value>
		///   The packages.
		/// </value>
		public override IEnumerable<KeyValuePair<string, Version>> Packages =>
			new[]
			{
				new KeyValuePair<string, Version>("DataSource", new Version(1, 0, 0, 3)),
				new KeyValuePair<string, Version>("DataSourceFields", new Version(1, 0, 0, 2)),
				new KeyValuePair<string, Version>("Templates", new Version(1, 0, 0, 3)),
				new KeyValuePair<string, Version>("SystemPackage", new Version(1, 5, 0, 0))
			};
	}
}