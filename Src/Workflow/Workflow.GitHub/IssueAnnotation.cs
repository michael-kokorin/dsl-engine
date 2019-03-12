namespace Workflow.GitHub
{
	/// <summary>
	///   Represents issue annotation.
	/// </summary>
	public sealed class IssueAnnotation
	{
		/// <summary>
		///   Gets or sets the file.
		/// </summary>
		/// <value>
		///   The file.
		/// </value>
		public string File { get; set; }

		/// <summary>
		///   Gets the link.
		/// </summary>
		/// <value>
		///   The link.
		/// </value>
		public string Link => File.Replace(" ", "%20").Replace("\\", "/");

		/// <summary>
		///   Gets or sets the line start.
		/// </summary>
		/// <value>
		///   The line start.
		/// </value>
		public int LineStart { get; set; }

		/// <summary>
		///   Gets or sets the identifier.
		/// </summary>
		/// <value>
		///   The identifier.
		/// </value>
		public string Id { get; set; }

		/// <summary>
		///   Gets or sets the severity.
		/// </summary>
		/// <value>
		///   The severity.
		/// </value>
		public string Severity { get; set; }

		/// <summary>
		///   Gets or sets the long name.
		/// </summary>
		/// <value>
		///   The long name.
		/// </value>
		public string LongName { get; set; }

		/// <summary>
		///   Gets or sets the issue path.
		/// </summary>
		/// <value>
		///   The issue path.
		/// </value>
		public string IssuePath { get; set; }

		/// <summary>
		///   Gets or sets the content of the additional.
		/// </summary>
		/// <value>
		///   The content of the additional.
		/// </value>
		public string AdditionalContent { get; set; }

		/// <summary>
		///   Gets or sets the state.
		/// </summary>
		/// <value>
		///   The state.
		/// </value>
		public IssueAnnotationState State { get; set; }

		/// <summary>
		///   Gets or sets the line end.
		/// </summary>
		/// <value>
		///   The line end.
		/// </value>
		public int LineEnd { get; set; }
	}
}