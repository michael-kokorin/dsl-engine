namespace Common.Extensions
{
	using System.Collections.Generic;
	using System.ComponentModel;

	/// <summary>
	///   Provides extension methods to work with object.
	/// </summary>
	public static class ObjectExtensions
	{
		/// <summary>
		///   Adds the property to list of properties of this object.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="name">The property name.</param>
		/// <param name="value">The property value.</param>
		/// <returns>Dictionary containing all properties of this object and the specified one.</returns>
		public static IDictionary<string, object> AddProperty(this object obj, string name, object value)
		{
			var dictionary = obj.ToDictionary();
			dictionary.Add(name, value);
			return dictionary;
		}

		/// <summary>
		///   Extracts all properties of this object into dictionary.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns>Dictionary containing all properties of this object.</returns>
		public static IDictionary<string, object> ToDictionary(this object obj)
		{
			IDictionary<string, object> result = new Dictionary<string, object>();

			var properties = TypeDescriptor.GetProperties(obj);

			foreach(PropertyDescriptor property in properties)
				result.Add(property.Name, property.GetValue(obj));

			return result;
		}
	}
}