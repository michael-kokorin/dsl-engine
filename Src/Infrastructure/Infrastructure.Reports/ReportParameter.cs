namespace Infrastructure.Reports
{
	using System.Xml.Serialization;

	/// <summary>
	/// Report parameter definition
	/// </summary>
	public sealed class ReportParameter
	{
		/// <summary>
		/// Gets or sets the report parameter name.
		/// </summary>
		/// <value>
		/// The report parameter name.
		/// </value>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the report parameter key.
		/// </summary>
		/// <value>
		/// The report parameter key.
		/// </value>
		[XmlAttribute]
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets the default parameter value value.
		/// </summary>
		/// <value>
		/// The report parameter default value.
		/// </value>
		[XmlAttribute]
		public string Value { get; set; }

		public ReportParameter(string name, string key, string value = null)
		{
			Name = name;
			Key = key;
			Value = value;
		}
	}
}