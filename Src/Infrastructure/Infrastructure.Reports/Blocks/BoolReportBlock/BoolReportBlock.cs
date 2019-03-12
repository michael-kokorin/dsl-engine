namespace Infrastructure.Reports.Blocks.BoolReportBlock
{
	public sealed class BoolReportBlock : IReportBlock
	{
		/// <summary>
		/// Gets or sets the report block identifier.
		/// </summary>
		/// <value>
		/// The report block identifier.
		/// </value>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the positive.
		/// </summary>
		/// <value>
		/// The positive.
		/// </value>
		public IReportBlock Positive { get; set; }

		/// <summary>
		/// Gets or sets the negative.
		/// </summary>
		/// <value>
		/// The negative.
		/// </value>
		public IReportBlock Negative { get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public string Variable { get; set; }
	}
}
