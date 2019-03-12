namespace Common.Enums
{
	/// <summary>
	///   Database column types
	/// </summary>
	public enum TableColumnType
	{
		/// <summary>
		///   Table column contains values
		/// </summary>
		Value = 0,

		/// <summary>
		///   Table column navigates to other tables
		/// </summary>
		Navigation = 1
	}
}