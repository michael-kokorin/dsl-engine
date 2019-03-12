namespace DbMigrations
{
	using System;

	using JetBrains.Annotations;

	/// <summary>
	///   Indicates modifiers for column.
	/// </summary>
	[Flags]
	internal enum ColumnProperty
	{
		/// <summary>
		///   The none
		/// </summary>
		[UsedImplicitly]
		None = 0,

		/// <summary>
		///   The null
		/// </summary>
		Null = 1,

		/// <summary>
		///   The not null
		/// </summary>
		NotNull = 2,

		/// <summary>
		///   The identity
		/// </summary>
		Identity = 4,

		/// <summary>
		///   The unique
		/// </summary>
		Unique = 8,

		/// <summary>
		///   The unsigned
		/// </summary>
		Unsigned = 16,

		/// <summary>
		///   The foreign key
		/// </summary>
		ForeignKey = 32,

		/// <summary>
		///   The primary key
		/// </summary>
		PrimaryKey = 64,

		/// <summary>
		///   The primary key with identity
		/// </summary>
		PrimaryKeyWithIdentity = 70
	}
}