namespace Workflow.GitHub
{
	/// <summary>
	///   Provide methods to get serialized value of issue annotation state.
	/// </summary>
	internal interface IIssueAnnotationStateSerializer
	{
		/// <summary>
		///   Gets the serialized value of the specified state.
		/// </summary>
		/// <param name="state">The state.</param>
		/// <returns>The serialized value.</returns>
		string Serialize(IssueAnnotationState state);

		/// <summary>
		///   Deserializes issue annotation state from the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>The issue annotation state.</returns>
		IssueAnnotationState Deserialize(string text);
	}
}