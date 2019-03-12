namespace Workflow.GitHub
{
	/// <summary>
	///   Indicates file change annotation type.
	/// </summary>
	public enum FileChangeAnnotationType
	{
		/// <summary>
		///   No change.
		/// </summary>
		None = 0,

		/// <summary>
		///   The add change.
		/// </summary>
		Add = 1,

		/// <summary>
		///   The update change.
		/// </summary>
		Update = 2
	}
}