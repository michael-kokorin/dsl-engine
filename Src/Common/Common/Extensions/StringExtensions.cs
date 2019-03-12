namespace Common.Extensions
{
	using System;
	using System.Globalization;
	using System.IO;
	using System.Text;

	using JetBrains.Annotations;

	/// <summary>
	///   Provides extension methods for strings.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		///   Checks whether two string are equal ignore case.
		/// </summary>
		/// <param name="first">The first.</param>
		/// <param name="second">The second.</param>
		/// <returns><see langword="true"/> if strings are equal; otherwise, <see langword="false"/>.</returns>
		public static bool EqualIgnoreCase(this string first, string second)
			=> string.Equals(first, second, StringComparison.OrdinalIgnoreCase);

		/// <summary>
		///   Formats string with the specified arguments.
		/// </summary>
		/// <param name="template">The format.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>Formatted string.</returns>
		public static string FormatWith(this string template, params object[] args)
			=> string.Format(CultureInfo.InvariantCulture, template, args);

		/// <summary>
		///   Replaces the first occurence of <paramref name="placeholder"/> by <paramref name="replacement"/>.
		/// </summary>
		/// <param name="template">The template.</param>
		/// <param name="placeholder">The placeholder.</param>
		/// <param name="replacement">The replacement.</param>
		/// <returns>String with replacement.</returns>
		public static string ReplaceOnce([NotNull] this string template, string placeholder, string replacement)
		{
			if (string.IsNullOrEmpty(placeholder))
				return template;

			var num = template.IndexOf(placeholder, StringComparison.Ordinal);
			if (num < 0)
				return template;

			return
				new StringBuilder(template.Substring(0, num))
					.Append(replacement)
					.Append(template.Substring(num + placeholder.Length))
					.ToString();
		}

		/// <summary>
		/// update string to the valid path
		/// </summary>
		/// <param name="source">The source string.</param>
		/// <returns>Path string</returns>
		public static string ToValidPath([NotNull] this string source)
		{
			var invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars()));

			var invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

			return System.Text.RegularExpressions.Regex.Replace(source, invalidRegStr, "_");
		}
	}
}