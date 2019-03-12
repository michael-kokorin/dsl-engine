namespace Workflow.GitHub
{
	using System;

	using JetBrains.Annotations;

	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
	internal sealed class SharpSingleLineIssueAnnotationFormatter: IIssueAnnotationFormatter
	{
		/// <summary>
		///   Formats the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>The formatted text.</returns>
		public string Format(string text) => "// " + text.Replace("\n", "\n// ");

		/// <summary>
		///   Verifies that line starts according to the formatting.
		/// </summary>
		/// <param name="line">The line.</param>
		/// <returns><see langword="true"/> if line starts according to the formatting; otherwise, <see langword="false"/>.</returns>
		public bool DoesItStartRight(string line) => line.Trim().StartsWith(@"//", StringComparison.Ordinal);

		/// <summary>
		///   Removes format from the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>Text without formatting.</returns>
		public string Deformat(string text) => text.TrimStart(' ', '\t', '/').TrimEnd(' ', '\t');
	}
}