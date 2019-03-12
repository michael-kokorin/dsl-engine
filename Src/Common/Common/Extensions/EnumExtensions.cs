namespace Common.Extensions
{
	using System;
	using System.ComponentModel;
	using System.Globalization;
	using System.Reflection;
	using System.Resources;

	/// <summary>
	///   Provides extension methods for enums.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		///   Gets the enum description.
		/// </summary>
		/// <param name="source">The source enum.</param>
		/// <returns>Enum description</returns>
		public static string GetDescription(this Enum source) =>
			source
				.GetType()
				.GetField(source.ToString())
				.GetCustomAttribute<DescriptionAttribute>()
				.Description;

		/// <summary>
		///   Gets the name of the enum from resources.
		/// </summary>
		/// <param name="value">The enum value.</param>
		/// <param name="resourceManager">The resource manager.</param>
		/// <param name="culture">The culture.</param>
		/// <returns>Localized enum name from resource manager</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="resourceManager"/> is <see langword="null"/>.</exception>
		public static string GetEnumName(this Enum value, ResourceManager resourceManager, CultureInfo culture = null)
		{
			if(resourceManager == null)
				throw new ArgumentNullException(nameof(resourceManager));

			if(culture == null)
				culture = CultureInfo.CurrentCulture;

			var valueType = value.GetType();

			var valInt64 = Convert.ToInt64(value);

			var name = Enum.GetName(valueType, valInt64);
			var valueName = resourceManager.GetString($"Enum_{valueType.Name}_{name}", culture);

			return valueName ?? name;
		}

		/// <summary>
		///   Gets the value from a different enum equal by value.
		/// </summary>
		/// <typeparam name="TResult">The type of the target enum.</typeparam>
		/// <param name="source">The source value.</param>
		/// <returns>The target value.</returns>
		/// <exception cref="System.ComponentModel.InvalidEnumArgumentException">
		///   Thrown when <typeparamref name="TResult"/> is not an enum.
		/// </exception>
		public static TResult GetEqualByValue<TResult>(this Enum source)
			where TResult: struct
		{
			if(!typeof(TResult).IsEnum)
				throw new InvalidEnumArgumentException(nameof(source), (int)(object)source, typeof(TResult));

			var sourceValue = source.GetHashCode();

			return (TResult)(object)sourceValue;
		}
	}
}