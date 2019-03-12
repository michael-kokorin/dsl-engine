namespace Workflow.GitHub
{
	using System.Collections.Generic;
	using System.IO;

	using JetBrains.Annotations;

	using Microsoft.Practices.ObjectBuilder2;

	[UsedImplicitly]
	internal sealed class IssueAnnotationsProvider: IAnnotationIssuesProvider
	{
		private readonly IIssueAnnotationFormatter _formatter;

		private readonly IIssueAnnotationSerializer _serializer;

		public IssueAnnotationsProvider(
			IIssueAnnotationFormatter formatter,
			IIssueAnnotationSerializer serializer)
		{
			_formatter = formatter;
			_serializer = serializer;
		}

		/// <summary>
		///   Gets the issues.
		/// </summary>
		/// <param name="repoPath">The repo path.</param>
		/// <param name="sourceCodeFiles">The source code files.</param>
		/// <returns>
		///   Issues.
		/// </returns>
		public IssueAnnotation[] GetIssues(string repoPath, string[] sourceCodeFiles)
		{
			var issues = new List<IssueAnnotation>();

			if(sourceCodeFiles.Length == 0)
				return issues.ToArray();

			foreach(var file in sourceCodeFiles)
			{
				if(string.IsNullOrEmpty(file))
					continue;

				var content = File.ReadAllLines(file);
				var fileIssues = _serializer.Deserialize(_formatter, content);
				fileIssues.ForEach(x => x.File = file.Replace(repoPath, string.Empty).TrimStart('\\'));
				issues.AddRange(fileIssues);
			}

			return issues.ToArray();
		}
	}
}