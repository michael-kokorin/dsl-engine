namespace DbMigrations
{
	using System;
	using System.Data;

	/// <summary>
	///   Represents database column.
	/// </summary>
	/// <seealso cref="DbMigrations.IColumn"/>
	internal sealed class Column: IColumn
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		protected Column()
		{
			ColumnType = new ColumnType(DbType.AnsiString);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public Column(string name) : this()
		{
			Name = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		public Column(string name, DbType type) : this()
		{
			Name = name;
			ColumnType.DataType = type;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="size">The size.</param>
		public Column(string name, DbType type, int size): this()
		{
			Name = name;
			ColumnType.DataType = type;
			ColumnType.Length = size;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="defaultValue">The default value.</param>
		public Column(string name, DbType type, object defaultValue): this(name, type)
		{
			DefaultValue = defaultValue;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="property">The property.</param>
		public Column(string name, DbType type, ColumnProperty property): this(name, type)
		{
			ColumnProperty = property;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="size">The size.</param>
		/// <param name="property">The property.</param>
		public Column(string name, DbType type, int size, ColumnProperty property): this(name, type, size)
		{
			ColumnProperty = property;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="size">The size.</param>
		/// <param name="property">The property.</param>
		/// <param name="defaultValue">The default value.</param>
		public Column(string name, DbType type, int size, ColumnProperty property, object defaultValue)
			: this(name, type, size)
		{
			ColumnProperty = property;
			DefaultValue = defaultValue;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="property">The property.</param>
		/// <param name="defaultValue">The default value.</param>
		public Column(string name, DbType type, ColumnProperty property, object defaultValue): this(name, type)
		{
			ColumnProperty = property;
			DefaultValue = defaultValue;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public Column(string name, ColumnType type)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));
			Name = name;
			ColumnType = type;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="property">The property.</param>
		public Column(string name, ColumnType type, ColumnProperty property): this(name, type)
		{
			ColumnProperty = property;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="defaultValue">The default value.</param>
		public Column(string name, ColumnType type, object defaultValue): this(name, type)
		{
			DefaultValue = defaultValue;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="Column"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="property">The property.</param>
		/// <param name="defaultValue">The default value.</param>
		public Column(string name, ColumnType type, ColumnProperty property, object defaultValue): this(name, type)
		{
			ColumnProperty = property;
			DefaultValue = defaultValue;
		}

		/// <summary>
		///   Gets or sets the column property.
		/// </summary>
		/// <value>
		///   The column property.
		/// </value>
		public ColumnProperty ColumnProperty { get; set; }

		/// <summary>
		///   Gets or sets the type of the column.
		/// </summary>
		/// <value>
		///   The type of the column.
		/// </value>
		public ColumnType ColumnType { get; set; }

		/// <summary>
		///   Gets or sets the default value.
		/// </summary>
		/// <value>
		///   The default value.
		/// </value>
		public object DefaultValue { get; set; }

		/// <summary>
		///   Gets a value indicating whether this instance is identity.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if this instance is identity; otherwise, <see langword="false"/>.
		/// </value>
		public bool IsIdentity => (ColumnProperty & ColumnProperty.Identity) == ColumnProperty.Identity;

		/// <summary>
		///   Gets a value indicating whether this instance is primary key.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if this instance is primary key; otherwise, <see langword="false"/>.
		/// </value>
		public bool IsPrimaryKey => (ColumnProperty & ColumnProperty.PrimaryKey) == ColumnProperty.PrimaryKey;

		/// <summary>
		///   Gets or sets the name.
		/// </summary>
		/// <value>
		///   The name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		///   Sets the type of the column.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
		public void SetColumnType(ColumnType type)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			ColumnType = type;
		}
	}
}