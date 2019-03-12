namespace Infrastructure.Events
{
	using System.Collections.Generic;
	using System.Xml;
	using System.Xml.Schema;
	using System.Xml.Serialization;

	public sealed class Event : IXmlSerializable
	{
		private const string ItemElementName = "Item";

		private const string KeyElementName = "Key";

		private const string ValueElementName = "Value";

		public string Key { get; set; }

		public IDictionary<string, string> Data { get; set; }

		public XmlSchema GetSchema() => null;

		public void ReadXml(XmlReader reader)
		{
			reader.MoveToContent();

			Key = reader.GetAttribute(nameof(Key));

			reader.Read();

			if (!reader.IsStartElement(nameof(Data)))
				return;

			Data = new Dictionary<string, string>();

			reader.ReadStartElement(nameof(Data));

			while (reader.NodeType != XmlNodeType.EndElement)
			{
				reader.ReadStartElement(ItemElementName);

				var key = reader.ReadElementString(KeyElementName);
				var value = reader.ReadElementString(ValueElementName);

				reader.ReadEndElement();
				reader.MoveToContent();

				Data.Add(key, value);
			}

			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString(nameof(Key), Key);

			if (Data == null) return;

			writer.WriteStartElement(nameof(Data));

			foreach (var key in Data.Keys)
			{
				var value = Data[key];

				writer.WriteStartElement(ItemElementName);

				writer.WriteElementString(KeyElementName, key);
				writer.WriteElementString(ValueElementName, value);

				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}
	}
}