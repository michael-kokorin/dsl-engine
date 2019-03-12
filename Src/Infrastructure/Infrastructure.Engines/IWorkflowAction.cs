namespace Infrastructure.Engines
{
	using System.Collections.Generic;

	/// <summary>
	///     Represents contract for workflow actions.
	/// </summary>
	public interface IWorkflowAction
	{
		/// <summary>
		///     Executes the action with specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		void Execute(IDictionary<string, string> parameters);
	}
}