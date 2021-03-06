﻿namespace DbMigrations.Migrations.Migration_53
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class MigrationPackageMap_53: MigrationPackagesMap
	{
		/// <summary>
		///     Gets the packages.
		/// </summary>
		/// <value>
		///     The packages.
		/// </value>
		public override IEnumerable<KeyValuePair<string, Version>> Packages =>
			new[]
			{
				new KeyValuePair<string, Version>("Reports", new Version(1, 0, 0, 5))
			};
	}
}