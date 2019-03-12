namespace Common.Enums
{
	/// <summary>
	///   Represents status of the task.
	/// </summary>
	public enum TaskStatus
	{
		/// <summary>
		///   New.
		/// </summary>
		New = 0,

		/// <summary>
		///   Pre-processing
		/// </summary>
		PreProcessing = 1,

		/// <summary>
		///   Ready to scan.
		/// </summary>
		ReadyToScan = 2,

		/// <summary>
		///   Scanning.
		/// </summary>
		Scanning = 3,

		/// <summary>
		///   Ready to post processing.
		/// </summary>
		ReadyToPostProcessing = 5,

		/// <summary>
		///   Post processing.
		/// </summary>
		PostProcessing = 6,

		/// <summary>
		///   Done.
		/// </summary>
		Done = 7
	}
}