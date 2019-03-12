namespace Infrastructure.Reports
{
	using System;
	using System.Xml.Serialization;

	using JetBrains.Annotations;

	/// <summary>
	/// Report query parameter
	/// </summary>
	public sealed class ReportQueryParameter
	{
		/// <summary>
		/// Gets or sets the query paremeter key.
		/// </summary>
		/// <value>
		/// The query parameter key.
		/// </value>
		[XmlAttribute]
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets the report query parameter value.
		/// </summary>
		/// <value>
		/// The report query parameter value.
		/// </value>
		[XmlAttribute]
		public string Value { get; set; }

		public ReportQueryParameter([NotNull] string key, [NotNull] string value)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));
			if (value == null) throw new ArgumentNullException(nameof(value));

			Key = key;
			Value = value;
		}
	}
}