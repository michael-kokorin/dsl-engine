namespace Infrastructure.Reports.Blocks.Container
{
	using System.Xml.Serialization;

	/// <summary>
	/// Container report block
	/// </summary>
	public sealed class ContainerReportBlock : ReportBlock
	{
		/// <summary>
		/// Gets or sets a break page before condition.
		/// </summary>
		/// <value>
		///   <c>true</c> if [break page before]; otherwise, <c>false</c>.
		/// </value>
		[XmlAttribute]
		public bool BreakPageBefore { get; set; }

		/// <summary>
		/// Gets or sets the child proportions.
		/// </summary>
		/// <value>
		/// The child proportions.
		/// </value>
		[XmlElement]
		public int[] ChildProportions { get; set; }

		/// <summary>
		/// Gets or sets the container child report blocks.
		/// </summary>
		/// <value>
		/// The container child report blocks.
		/// </value>
		[XmlElement]
		public IReportBlock[] Childs { get; set; }

		/// <summary>
		/// Gets or sets the orientation of child block output.
		/// 
		/// By default, oriented by vertical.
		/// </summary>
		/// <value>
		/// The container block orientation.
		/// </value>
		[XmlAttribute]
		public ContainerOrientation Orientation { get; set; }

		public ContainerReportBlock() : this("Container")
		{

		}

		public ContainerReportBlock(string id)
		{
			BreakPageBefore = false;

			Id = id;

			Orientation = ContainerOrientation.Vertical;
		}
	}
}