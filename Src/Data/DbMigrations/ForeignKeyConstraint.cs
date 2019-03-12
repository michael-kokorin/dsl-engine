namespace DbMigrations
{
	/// <summary>
	///   Indicates foreign key constraint.
	/// </summary>
	internal enum ForeignKeyConstraint
	{
		/// <summary>
		///   The cascade
		/// </summary>
		Cascade,

		/// <summary>
		///   The set null
		/// </summary>
		SetNull,

		/// <summary>
		///   The no action
		/// </summary>
		NoAction,

		/// <summary>
		///   The restrict
		/// </summary>
		Restrict,

		/// <summary>
		///   The set default
		/// </summary>
		SetDefault
	}
}