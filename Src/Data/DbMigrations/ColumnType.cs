namespace DbMigrations
{
	using System.Data;

	using JetBrains.Annotations;

	/// <summary>
	///   Represents column type.
	/// </summary>
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	internal sealed class ColumnType
	{
		/// <summary>
		///   Initializes a new instance of the <see cref="ColumnType"/> class.
		/// </summary>
		/// <param name="dataType">Type of the data.</param>
		public ColumnType(DbType dataType)
		{
			DataType = dataType;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="ColumnType"/> class.
		/// </summary>
		/// <param name="dataType">Type of the data.</param>
		/// <param name="length">The length.</param>
		public ColumnType(DbType dataType, int length): this(dataType)
		{
			Length = length;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="ColumnType"/> class.
		/// </summary>
		/// <param name="dataType">Type of the data.</param>
		/// <param name="length">The length.</param>
		/// <param name="scale">The scale.</param>
		public ColumnType(DbType dataType, int length, int scale): this(dataType, length)
		{
			Scale = scale;
		}

		/// <summary>
		///   Gets or sets the type of the data.
		/// </summary>
		/// <value>
		///   The type of the data.
		/// </value>
		public DbType DataType { get; set; }

		/// <summary>
		///   Gets or sets the length.
		/// </summary>
		/// <value>
		///   The length.
		/// </value>
		public int? Length { get; set; }

		/// <summary>
		///   Gets or sets the scale.
		/// </summary>
		/// <value>
		///   The scale.
		/// </value>
		public int? Scale { get; set; }

		/// <summary>
		///   Performs an implicit conversion from <see cref="DbType"/> to <see cref="ColumnType"/>.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		///   The result of the conversion.
		/// </returns>
		public static implicit operator ColumnType(DbType type) => new ColumnType(type);

		/// <summary>
		///   Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		///   A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			string str;
			if(!Length.HasValue || !Scale.HasValue)
				str = Length.HasValue ? $"({Length})" : string.Empty;
			else
				str = $"({Length}, {Scale})";
			return $"{DataType}{str}";
		}
	}
}