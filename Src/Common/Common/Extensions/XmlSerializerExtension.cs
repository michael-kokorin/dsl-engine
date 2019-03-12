namespace Common.Extensions
{
	using System;
	using System.IO;
	using System.Text;
	using System.Xml;
	using System.Xml.Serialization;

	/// <summary>
	///   Provides extension methods to perform xml serialization.
	/// </summary>
	public static class XmlSerializerExtension
	{
		/// <summary>
		///   Deserializes entity from xml.
		/// </summary>
		/// <typeparam name="T">Type of entity.</typeparam>
		/// <param name="xmlValue">The XML content of entity.</param>
		/// <returns>Deserialized entity.</returns>
		public static T FromXml<T>(this string xmlValue) => (T)FromXml(xmlValue, typeof(T));

		private static object FromXml(this string xmlValue, Type type)
		{
			using(var reader = new StringReader(xmlValue))
				return new XmlSerializer(type).Deserialize(reader);
		}

		/// <summary>
		///   Serializes entity to xml.
		/// </summary>
		/// <typeparam name="T">Type of entity.</typeparam>
		/// <param name="entity">The entity.</param>
		/// <returns>Xml-serialized entity.</returns>
		public static string ToXml<T>(this T entity)
		{
			if(entity.Equals(default(T)))
				return null;

			var sb = new StringBuilder();

			using(var writer = XmlWriter.Create(
				sb,
				new XmlWriterSettings
				{
					OmitXmlDeclaration = true,
					Indent = true
				}))
			{
				var nsSerializer = new XmlSerializerNamespaces();

				nsSerializer.Add(string.Empty, string.Empty);

				new XmlSerializer(entity.GetType()).Serialize(writer, entity, nsSerializer);
			}

			return sb.ToString();
		}
	}
}