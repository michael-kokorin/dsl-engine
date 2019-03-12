namespace Workflow.GitHub
{
	using JetBrains.Annotations;

	using Common.Extensions;
	using Workflow.GitHub.Properties;

	/// <summary>
	///   Builds branch name by the specified information.
	/// </summary>
	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
	internal sealed class IssueNameBuilder: IIssueNameBuilder
	{
		/// <summary>
		///   Builds the issue name by the specified information.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <returns>The issue name.</returns>
		public string Build(IssueNameBuilderInfo info) =>
			Resources.IssueNameFormat.FormatWith(info.IssueTypeName);
	}
}