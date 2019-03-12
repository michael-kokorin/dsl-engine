namespace DbMigrations
{
	/// <summary>
	///   Represents column contract.
	/// </summary>
	internal interface IColumn
	{
		/// <summary>
		///   Gets or sets the column property.
		/// </summary>
		/// <value>
		///   The column property.
		/// </value>
		ColumnProperty ColumnProperty { get; set; }

		/// <summary>
		///   Gets the type of the column.
		/// </summary>
		/// <value>
		///   The type of the column.
		/// </value>
		ColumnType ColumnType { get; }

		/// <summary>
		///   Gets or sets the default value.
		/// </summary>
		/// <value>
		///   The default value.
		/// </value>
		object DefaultValue { get; set; }

		/// <summary>
		///   Gets a value indicating whether this instance is identity.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if this instance is identity; otherwise, <see langword="false"/>.
		/// </value>
		bool IsIdentity { get; }

		/// <summary>
		///   Gets a value indicating whether this instance is primary key.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if this instance is primary key; otherwise, <see langword="false"/>.
		/// </value>
		bool IsPrimaryKey { get; }

		/// <summary>
		///   Gets or sets the name.
		/// </summary>
		/// <value>
		///   The name.
		/// </value>
		string Name { get; set; }

		/// <summary>
		///   Sets the type of the column.
		/// </summary>
		/// <param name="type">The type.</param>
		void SetColumnType(ColumnType type);
	}
}