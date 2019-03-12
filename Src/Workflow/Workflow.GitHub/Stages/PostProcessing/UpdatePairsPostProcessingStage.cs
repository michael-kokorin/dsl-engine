namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class UpdatePairsPostProcessingStage : PostProcessingStage
	{
		private readonly IIssueAnnotationWorkflow _issueAnnotationWorkflow;

		public UpdatePairsPostProcessingStage([NotNull] IIssueAnnotationWorkflow issueAnnotationWorkflow)
		{
			if (issueAnnotationWorkflow == null) throw new ArgumentNullException(nameof(issueAnnotationWorkflow));

			_issueAnnotationWorkflow = issueAnnotationWorkflow;
		}

		protected override Func<Tasks, bool> PreCondition => _ => true;

		protected override void ExecuteStage(PostProcessingBundle bundle)
		{
			var pairs = new List<IssueVulnerabilityLink>();

			var vulnGroups = bundle.VulnerabilitiesInfo
				.GroupBy(x => x.Type)
				.ToDictionary(x => x.Key, x => x.ToArray());

			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (var issueAnnotation in bundle.IssueAnnotations)
			{
				if (!vulnGroups.ContainsKey(issueAnnotation.LongName))
					continue;

				var vulns = vulnGroups[issueAnnotation.LongName];
				var vulnerability =
					vulns
						.Where(x => x.File == issueAnnotation.File)
						.FirstOrDefault(x => issueAnnotation.LineEnd + 1 == x.NumberLine);
				if (vulnerability == null)
					continue;

				vulnerability.IssueNumber = issueAnnotation.Id;
				vulnerability.IssueUrl = issueAnnotation.IssuePath;

				pairs.Add(new IssueVulnerabilityLink(issueAnnotation, vulnerability));
			}

			// reopen or fp
			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var pair in pairs)
			{
				// ignoring false positives
				if (pair.IssueAnnotation.State == IssueAnnotationState.FalsePositive)
					continue;

				// Don't process not finished issues
				if ((pair.IssueAnnotation.State != IssueAnnotationState.Verify) &&
					(pair.IssueAnnotation.State != IssueAnnotationState.Fixed))
				{
					bundle.Changes.Add(new FileChangeAnnotation
					{
						Annotation = pair.IssueAnnotation,
						Vulnerability = pair.VulnerabilityInfo,
						Type =
							pair.IssueAnnotation.State == IssueAnnotationState.Todo
								? FileChangeAnnotationType.None
								: FileChangeAnnotationType.Update
					});
					continue;
				}

				_issueAnnotationWorkflow.ChangeState(pair.IssueAnnotation, IssueAnnotationState.Reopen);

				bundle.Changes.Add(new FileChangeAnnotation
				{
					Annotation = pair.IssueAnnotation,
					Vulnerability = pair.VulnerabilityInfo,
					Type = FileChangeAnnotationType.Update
				});
			}

			bundle.IssueVulnerabilityLinks = pairs.ToArray();
		}
	}
}