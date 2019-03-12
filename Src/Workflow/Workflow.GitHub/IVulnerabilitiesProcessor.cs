namespace Workflow.GitHub
{
	using Repository.Context;

	/// <summary>
	///   Provides methods to process vulnerabilities.
	/// </summary>
	public interface IVulnerabilitiesProcessor
	{
		/// <summary>
		///   Processes the specified vulnerabilities.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <returns>
		///   Actual vulnerabilities.
		/// </returns>
		TaskProcessingResult Process(Tasks task);
	}
}