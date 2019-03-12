namespace DbMigrations.Migrations.Migration_39
{
	using System;
	using System.Collections.Generic;

	// ReSharper disable once InconsistentNaming
	internal sealed class MigrationPackageMap_39 : MigrationPackagesMap
	{
		/// <summary>
		///   Gets the packages.
		/// </summary>
		/// <value>
		///   The packages.
		/// </value>
		public override IEnumerable<KeyValuePair<string, Version>> Packages => new[]
		{
			new KeyValuePair<string, Version>("Reports", new Version(1, 0, 0, 2))
		};
	}
}