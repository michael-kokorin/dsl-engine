namespace Workflow.GitHub
{
	/// <summary>
	///   Provides methods to manage workflow of <see cref="IssueAnnotation"/>.
	/// </summary>
	internal interface IIssueAnnotationWorkflow
	{
		/// <summary>
		///   Changes the state of the annotation.
		/// </summary>
		/// <param name="annotation">The annotation.</param>
		/// <param name="toState">State of the annotation to change to.</param>
		void ChangeState(IssueAnnotation annotation, IssueAnnotationState toState);
	}
}