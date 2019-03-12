namespace Workflow.GitHub
{
	/// <summary>
	///   Provide methods to annotate issue annotation.
	/// </summary>
	internal interface IIssueAnnotationFormatter
	{
		/// <summary>
		///   Formats the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>The formatted text.</returns>
		string Format(string text);

		/// <summary>
		///   Verifies that line starts according to the formatting.
		/// </summary>
		/// <param name="line">The line.</param>
		/// <returns><see langword="true"/> if line starts according to the formatting; otherwise, <see langword="false"/>.</returns>
		bool DoesItStartRight(string line);

		/// <summary>
		///   Removes format from the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>Text without formatting.</returns>
		string Deformat(string text);
	}
}