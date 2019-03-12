namespace DbMigrations.Migrations.Migration_45
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	// ReSharper disable once InconsistentNaming
	[UsedImplicitly]
	internal sealed class MigrationPackageMap_45 : MigrationPackagesMap
	{
		/// <summary>
		///   Gets the packages.
		/// </summary>
		/// <value>
		///   The packages.
		/// </value>
		public override IEnumerable<KeyValuePair<string, Version>> Packages => new[]
		{
			new KeyValuePair<string, Version>("Templates", new Version(1, 0, 0, 6)),
			new KeyValuePair<string, Version>("Notifications", new Version(1, 0, 0, 3))
		};
	}
}