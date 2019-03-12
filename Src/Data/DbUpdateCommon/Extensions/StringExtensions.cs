namespace DbUpdateCommon.Extensions
{
	using System;

	using JetBrains.Annotations;

	public static class StringExtensions
	{
		private const string Null = "NULL";

		/// <summary>
		///   Quotes column name if needed.
		/// </summary>
		/// <param name="columnName">Name of the column.</param>
		/// <returns>Quoted column name.</returns>
		[UsedImplicitly]
		public static string QuoteIfNeeded(this string columnName) => $"[{columnName}]";

		/// <summary>
		///   Quotes the values.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		[UsedImplicitly]
		public static string[] Quote(this string[] values) =>
			Array.ConvertAll(values,
				val =>
					val == null ? Null : val.StartsWith("(", StringComparison.Ordinal) ? val : val.Quote());

		[UsedImplicitly]
		public static string Quote(this string value) => value == null ? Null : $"N'{value.Replace("'", "''")}'";
	}
}