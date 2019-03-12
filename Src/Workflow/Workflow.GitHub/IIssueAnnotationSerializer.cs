namespace Workflow.GitHub
{
	/// <summary>
	///   Provide methods to parse <see cref="IssueAnnotation"/>.
	/// </summary>
	internal interface IIssueAnnotationSerializer
	{
		/// <summary>
		///   Parses <see cref="IssueAnnotation"/> from the specified text.
		/// </summary>
		/// <param name="formatter">The formatter.</param>
		/// <param name="text">The text.</param>
		/// <returns>
		///   Parsed entity.
		/// </returns>
		IssueAnnotation[] Deserialize(IIssueAnnotationFormatter formatter, string[] text);

		/// <summary>
		///   Serializes the specified issue annotation.
		/// </summary>
		/// <param name="issueAnnotation">The issue annotation.</param>
		/// <returns></returns>
		string Serialize(IssueAnnotation issueAnnotation);
	}
}