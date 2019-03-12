namespace DbMigrations.Migrations.Migration_4
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class MigrationPackagesMap_4: MigrationPackagesMap
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
				new KeyValuePair<string, Version>("System", new Version(1, 0, 0, 0)),
				new KeyValuePair<string, Version>("Templates", new Version(1, 0, 0, 0)),
				new KeyValuePair<string, Version>("Configuration", new Version(1, 0, 0, 0))
			};
	}
}