namespace Infrastructure.Reports.Blocks
{
	using System;
	using System.Globalization;
	using System.Xml.Serialization;

	public sealed class ReportItemColor
	{
		private static readonly Random Random = new Random();

		/// <summary>
		/// Gets or sets the alpha.
		/// </summary>
		/// <value>
		/// The alpha.
		/// </value>
		[XmlAttribute]
		public float Alpha { get; set; }

		/// <summary>
		/// Gets or sets the red.
		/// </summary>
		/// <value>
		/// The red.
		/// </value>
		[XmlAttribute]
		public int Red { get; set; }

		/// <summary>
		/// Gets or sets the green.
		/// </summary>
		/// <value>
		/// The green.
		/// </value>
		[XmlAttribute]
		public int Green { get; set; }

		/// <summary>
		/// Gets or sets the blue.
		/// </summary>
		/// <value>
		/// The blue.
		/// </value>
		[XmlAttribute]
		public int Blue { get; set; }

		public ReportItemColor() : this(1.0f)
		{

		}

		public ReportItemColor(float alpha)
			: this(Random.Next(0, 255), Random.Next(0, 255), Random.Next(0, 255), alpha)
		{

		}

		public ReportItemColor(int red, int green, int blue, float alpha = 1.0f)
		{
			Red = red;

			Green = green;

			Blue = blue;

			Alpha = alpha;
		}

		public string ToHex() => $"#{Red.ToString("X")}{Green.ToString("X")}{Blue.ToString("X")}";

		public string ToRgb() => $"RGB({Red}, {Green}, {Blue})";

		public string ToRgba() => $"rgba({Red}, {Green}, {Blue}, {Alpha.ToString("F1", CultureInfo.InvariantCulture)})";
	}
}