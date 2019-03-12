namespace Common.Extensions
{
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	///   Provides extension methods to work with dictionary.
	/// </summary>
	public static class DictionaryExtension
	{
		/// <summary>
		///   Gets the value by the specified key.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="key">The key.</param>
		/// <returns>The value with the specified key.</returns>
		public static TResult Get<TKey, TResult>(this IDictionary<TKey, TResult> source, TKey key)
			=> source.ContainsKey(key) ? source[key] : default(TResult);

		/// <summary>
		///   Concats two dictionaries and takes the first value for the same key.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="target">The target.</param>
		/// <returns>The result of concatenation of two dictionaries.</returns>
		public static IDictionary<TKey, TValue> ConcatWith<TKey, TValue>(
			this IDictionary<TKey, TValue> source,
			IDictionary<TKey, TValue> target) => source
				.Concat(target)
				.GroupBy(_ => _.Key)
				.ToDictionary(_ => _.Key, _ => _.First().Value);
	}
}