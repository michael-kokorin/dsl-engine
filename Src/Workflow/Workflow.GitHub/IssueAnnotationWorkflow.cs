namespace Workflow.GitHub
{
	using System;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Workflow.GitHub.Properties;

	[UsedImplicitly]
	internal sealed class IssueAnnotationWorkflow: IIssueAnnotationWorkflow
	{
		private readonly IIssueAnnotationStateSerializer _serializer;

		public IssueAnnotationWorkflow(IIssueAnnotationStateSerializer serializer)
		{
			_serializer = serializer;
		}

		/// <summary>
		///   Changes the state of the annotation.
		/// </summary>
		/// <param name="annotation">The annotation.</param>
		/// <param name="toState">State of the annotation to change to.</param>
		public void ChangeState(IssueAnnotation annotation, IssueAnnotationState toState)
		{
			if(annotation.State == toState)
				return;

			switch(annotation.State)
			{
				case IssueAnnotationState.FalsePositive:
					throw new InvalidOperationException(
						Resources.AnnotationStateCantBeChanged.FormatWith(
							_serializer.Serialize(annotation.State),
							_serializer.Serialize(toState)));
				case IssueAnnotationState.Fixed:
					if(toState != IssueAnnotationState.Reopen)
					{
						throw new InvalidOperationException(
							Resources.AnnotationStateCantBeChanged.FormatWith(
								_serializer.Serialize(annotation.State),
								_serializer.Serialize(toState)));
					}

					break;
				case IssueAnnotationState.Reopen:
					if(toState != IssueAnnotationState.Fixed)
					{
						throw new InvalidOperationException(
							Resources.AnnotationStateCantBeChanged.FormatWith(
								_serializer.Serialize(annotation.State),
								_serializer.Serialize(toState)));
					}

					break;
				case IssueAnnotationState.Todo:
					if(toState != IssueAnnotationState.Fixed)
					{
						throw new InvalidOperationException(
							Resources.AnnotationStateCantBeChanged.FormatWith(
								_serializer.Serialize(annotation.State),
								_serializer.Serialize(toState)));
					}

					break;

				case IssueAnnotationState.Verify:
					if((toState != IssueAnnotationState.Fixed) && (toState != IssueAnnotationState.Reopen))
					{
						throw new InvalidOperationException(
							Resources.AnnotationStateCantBeChanged.FormatWith(
								_serializer.Serialize(annotation.State),
								_serializer.Serialize(toState)));
					}

					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			annotation.State = toState;
		}
	}
}