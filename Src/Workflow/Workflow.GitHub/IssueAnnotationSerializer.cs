namespace Workflow.GitHub
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Logging;
	using Workflow.GitHub.Properties;

	[UsedImplicitly]
	internal sealed class IssueAnnotationSerializer: IIssueAnnotationSerializer
	{
		// TODO: AI issue #3, Medium, Arbitrary File Read, https://github.com/userwithrepo/ACSI_S4/issues/3
		private static readonly Regex AnnotationRegex =
			new Regex(
				"\\s*(?<marker>[\\w]+):\\s+AI\\s+issue\\s+#(?<id>\\d+),\\s+(?<severity>[^,]+),\\s+(?<longName>[^,]+),\\s+(?<url>[^\\s]+)");

		private readonly IIssueAnnotationStateSerializer _stateSerializer;

		private readonly ILog _logger;

		public IssueAnnotationSerializer(
			[NotNull] IIssueAnnotationStateSerializer stateSerializer,
			[NotNull] ILog logger)
		{
			if(stateSerializer == null) throw new ArgumentNullException(nameof(stateSerializer));
			if(logger == null) throw new ArgumentNullException(nameof(logger));

			_stateSerializer = stateSerializer;
			_logger = logger;
		}

		/// <summary>
		///   Parses <see cref="IssueAnnotation"/> from the specified text.
		/// </summary>
		/// <param name="formatter">The formatter.</param>
		/// <param name="text">The text.</param>
		/// <returns>
		///   Parsed entity.
		/// </returns>
		public IssueAnnotation[] Deserialize(IIssueAnnotationFormatter formatter, string[] text)
		{
			var result = new List<IssueAnnotation>();
			for(var lineIndex = 0; lineIndex < text.Length; lineIndex++)
			{
				var contentLine = text[lineIndex];
				if(!formatter.DoesItStartRight(contentLine))
					continue;

				var match = AnnotationRegex.Match(formatter.Deformat(contentLine));
				if(!match.Success)
					continue;

				var line = lineIndex + 1;
				var additionalСontent = string.Empty;
				while((lineIndex < text.Length) &&
							formatter.DoesItStartRight(text[lineIndex + 1]))
				{
					lineIndex++;
					additionalСontent += "\n" + formatter.Deformat(text[lineIndex]);
				}

				IssueAnnotationState state;
				try
				{
					state = _stateSerializer.Deserialize(match.Groups["marker"].Value);
				}
				catch(ArgumentException)
				{
					_logger.Error(
						Resources.IssueAnnotationSerializer_Deserialize_Wrong_marker_found.FormatWith(match.Groups["marker"].Value));
					continue;
				}

				result.Add(
					new IssueAnnotation
					{
						AdditionalContent = additionalСontent,
						Id = match.Groups["id"].Value,
						IssuePath = match.Groups["url"].Value,
						LineStart = line,
						LongName = match.Groups["longName"].Value,
						State = state,
						Severity = match.Groups["severity"].Value,
						LineEnd = lineIndex + 1
					});
			}

			return result.ToArray();
		}

		/// <summary>
		///   Serializes the specified issue annotation.
		/// </summary>
		/// <param name="issueAnnotation">The issue annotation.</param>
		/// <returns></returns>
		public string Serialize(IssueAnnotation issueAnnotation)
		{
			var result =
				$"{_stateSerializer.Serialize(issueAnnotation.State)}: AI issue #{issueAnnotation.Id}, {issueAnnotation.Severity}, {issueAnnotation.LongName}, {issueAnnotation.IssuePath}";

			if(string.IsNullOrWhiteSpace(issueAnnotation.AdditionalContent)) return result;

			var content = issueAnnotation.AdditionalContent;
			if(!content.StartsWith("\n", StringComparison.Ordinal))
				content = "\n" + content;

			result += content;

			return result;
		}
	}
}