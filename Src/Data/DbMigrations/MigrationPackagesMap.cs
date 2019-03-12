namespace DbMigrations
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	/// <summary>
	///     Associates packages with a specific migration version.
	/// </summary>
	internal abstract class MigrationPackagesMap
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="MigrationPackagesMap"/> class.
		/// </summary>
		/// <exception cref="TypeInitializationException"></exception>
		protected MigrationPackagesMap()
		{
			try
			{
				MigrationVersion = int.Parse(GetType().Name.Split('_').Skip(1).FirstOrDefault() ?? string.Empty);
			}
			catch(Exception exception)
			{
				throw new TypeInitializationException(GetType().FullName, exception);
			}
		}

		/// <summary>
		///     Gets the packages.
		/// </summary>
		/// <value>
		///     The packages.
		/// </value>
		[NotNull]
		public abstract IEnumerable<KeyValuePair<string, Version>> Packages { get; }

		/// <summary>
		///     Gets the migration version.
		/// </summary>
		/// <value>
		///     The migration version.
		/// </value>
		public int MigrationVersion { get; private set; }
	}
}