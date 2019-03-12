namespace Infrastructure.Engines.Dsl
{
	using System.Collections.Generic;

	/// <summary>
	///   Represents report attachment.
	/// </summary>
	public sealed class ReportAttachmentBlockExpr : INotificationAttachBlock
	{
		/// <summary>
		///   Initializes a new instance of the <see cref="ReportAttachmentBlockExpr"/> class.
		/// </summary>
		/// <param name="reportId">The report identifier.</param>
		/// <param name="exportFormat">The export format.</param>
		/// <param name="parameters">The parameters.</param>
		public ReportAttachmentBlockExpr(long reportId, string exportFormat, IEnumerable<KeyValuePairExpr> parameters)
		{
			ReportId = reportId;
			ExportFormat = exportFormat;
			Parameters = parameters;
		}

		/// <summary>
		///   Gets the export format.
		/// </summary>
		/// <value>
		///   The export format.
		/// </value>
		public string ExportFormat { get; }

		/// <summary>
		///   Gets the parameters.
		/// </summary>
		/// <value>
		///   The parameters.
		/// </value>
		public IEnumerable<KeyValuePairExpr> Parameters { get; }

		/// <summary>
		///   Gets the report identifier.
		/// </summary>
		/// <value>
		///   The report identifier.
		/// </value>
		public long ReportId { get; }
	}
}