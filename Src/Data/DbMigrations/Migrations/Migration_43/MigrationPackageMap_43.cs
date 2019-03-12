namespace DbMigrations.Migrations.Migration_43
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	[UsedImplicitly]
	// ReSharper disable once InconsistentNaming
	internal sealed class MigrationPackageMap_43 : MigrationPackagesMap
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
				new KeyValuePair<string, Version>("System", new Version(1, 6, 0, 0))
			};
	}
}