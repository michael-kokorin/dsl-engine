namespace Infrastructure.Engines.Dsl
{
	using System.Collections.Generic;

	/// <summary>
	///   Interface to mark notification attachment block.
	/// </summary>
	public interface INotificationAttachBlock
	{
		/// <summary>
		///   Gets the export format.
		/// </summary>
		/// <value>
		///   The export format.
		/// </value>
		string ExportFormat { get; }

		/// <summary>
		///   Gets the parameters.
		/// </summary>
		/// <value>
		///   The parameters.
		/// </value>
		IEnumerable<KeyValuePairExpr> Parameters { get; }

		/// <summary>
		///   Gets the report identifier.
		/// </summary>
		/// <value>
		///   The report identifier.
		/// </value>
		long ReportId { get; }
	}
}