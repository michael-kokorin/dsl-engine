namespace DbMigrations
{
	/// <summary>
	///   Provides extension methods for column property.
	/// </summary>
	internal static class ColumnPropertyExtensions
	{
		/// <summary>
		///   Determines whether the specified column has property.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="comparison">The comparison.</param>
		/// <returns><see langword="true"/> if the column has property; otherwise, <see langword="false"/>.</returns>
		public static bool HasProperty(this ColumnProperty source, ColumnProperty comparison)
			=> (source & comparison) == comparison;
	}
}