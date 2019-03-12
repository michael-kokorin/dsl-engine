namespace Infrastructure.Reports.Blocks.Label
{
	using System.Xml.Serialization;

	[XmlRoot]
	public sealed class LabelFontStyle
	{
		[XmlAttribute]
		public string FontFamily { get; set; }

		[XmlAttribute]
		public int FontSizePx { get; set; }

		[XmlAttribute]
		public bool Bold { get; set; }

		[XmlAttribute]
		public bool Italic { get; set; }

		public LabelFontStyle() : this(14)
		{

		}

		public LabelFontStyle(int fontSizePx) : this("Tahoma", fontSizePx, false, false)
		{

		}

		public LabelFontStyle(string fontFamily, int fontSizePx, bool bold, bool italic)
		{
			Bold = bold;

			FontFamily = fontFamily;

			FontSizePx = fontSizePx;

			Italic = italic;
		}
	}
}