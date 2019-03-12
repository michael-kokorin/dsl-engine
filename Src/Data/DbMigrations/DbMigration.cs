namespace DbMigrations
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	/// <summary>
	///     Represents database migration.
	/// </summary>
	internal abstract class DbMigration
	{
		protected DbMigration()
		{
			try
			{
				Version = int.Parse(GetType().Name.Split('_').Skip(1).FirstOrDefault() ?? string.Empty);
			}
			catch(Exception exception)
			{
				throw new TypeInitializationException(GetType().FullName, exception);
			}
		}

		/// <summary>
		///     Is transaction required for migration
		/// </summary>
		public virtual bool RequireTransaction => true;

		/// <summary>
		///     Gets the version.
		/// </summary>
		/// <value>
		///     The version.
		/// </value>
		public int Version { get; private set; }

		/// <summary>
		///     Performs migration.
		/// </summary>
		public abstract void Up([NotNull] IDbTransformationProvider database);
	}
}