namespace Infrastructure.Telemetry
{
	/// <summary>
	///     Telemetry operation statuses
	/// </summary>
	public enum TelemetryOperationStatus
	{
		/// <summary>
		///     The successfull operation result status
		/// </summary>
		Ok = 0,

		/// <summary>
		///     Operation execution finished with unhandled exception
		/// </summary>
		Exception = 1
	}
}