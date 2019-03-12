namespace Workflow.GitHub
{
	using System;
	using System.ComponentModel;

	using JetBrains.Annotations;

	using Workflow.GitHub.Properties;

	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
	internal sealed class IssueAnnotationStateSerializer: IIssueAnnotationStateSerializer
	{
		/// <summary>
		///   Gets the serialized value of the specified state.
		/// </summary>
		/// <param name="state">The state.</param>
		/// <returns>The serialized value.</returns>
		public string Serialize(IssueAnnotationState state)
		{
			switch(state)
			{
				case IssueAnnotationState.FalsePositive:
					return Resources.FalsePositiveMarker;
				case IssueAnnotationState.Fixed:
					return Resources.FixedMarker;
				case IssueAnnotationState.Reopen:
					return Resources.ReopenMarker;
				case IssueAnnotationState.Todo:
					return Resources.TodoMarker;
				case IssueAnnotationState.Verify:
					return Resources.VerifyMarker;
				default:
					throw new InvalidEnumArgumentException(nameof(state));
			}
		}

		/// <summary>
		///   Deserializes issue annotation state from the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>The issue annotation state.</returns>
		public IssueAnnotationState Deserialize(string text)
		{
			if(text == Resources.FalsePositiveMarker)
				return IssueAnnotationState.FalsePositive;

			if(text == Resources.FixedMarker)
				return IssueAnnotationState.Fixed;

			if(text == Resources.ReopenMarker)
				return IssueAnnotationState.Reopen;

			if(text == Resources.TodoMarker)
				return IssueAnnotationState.Todo;

			if(text == Resources.VerifyMarker)
				return IssueAnnotationState.Verify;

			throw new ArgumentException(nameof(text));
		}
	}
}