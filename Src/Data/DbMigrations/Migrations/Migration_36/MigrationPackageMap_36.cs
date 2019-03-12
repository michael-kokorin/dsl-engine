namespace DbMigrations.Migrations.Migration_36
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	[UsedImplicitly]
	// ReSharper disable once InconsistentNaming
	internal sealed class MigrationPackageMap_36 : MigrationPackagesMap
	{
		/// <summary>
		///   Gets the packages.
		/// </summary>
		/// <value>
		///   The packages.
		/// </value>
		public override IEnumerable<KeyValuePair<string, Version>> Packages => new[]
		{
			new KeyValuePair<string, Version>("Templates", new Version(1, 0, 0, 5)),
			new KeyValuePair<string, Version>("Queries", new Version(1, 0, 0, 5)),
			new KeyValuePair<string, Version>("Notifications", new Version(1, 0, 0, 1))
		};
	}
}