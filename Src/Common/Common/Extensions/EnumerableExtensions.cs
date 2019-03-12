namespace Common.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	///   Provide extensions for <see cref="IEnumerable{T}"/> collections.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		///   Converts the collection to the comma separated string.
		/// </summary>
		/// <typeparam name="T">Type of collection item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>Comma separated string.</returns>
		public static string ToCommaSeparatedString<T>(this IEnumerable<T> collection)
			=> collection.ToSeparatedString();

		/// <summary>
		///   Converts the collection to the separated string.
		/// </summary>
		/// <typeparam name="T">Type of collection item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="separator">The separator.</param>
		/// <returns>Separated string.</returns>
		public static string ToSeparatedString<T>(this IEnumerable<T> collection, string separator = ", ")
			=> collection != null
				? string.Join(separator,
					(from el in collection
						select el.ToString()).ToArray())
				: null;

		/// <summary>
		///   Filters collection when condition is satisfied.
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="query">The query.</param>
		/// <param name="condition">
		///   if set to <see langword="true"/> then filter collection; otherwise, collection is not
		///   filtered.
		/// </param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>Filtered collection.</returns>
		public static IEnumerable<T> WhereIf<T>(
			this IEnumerable<T> query,
			bool condition,
			Func<T, bool> predicate) => condition ? query.Where(predicate) : query;
	}
}