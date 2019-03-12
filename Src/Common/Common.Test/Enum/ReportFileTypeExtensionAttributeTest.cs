namespace Common.Tests.Enum
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Enums;

	[TestFixture]
	public sealed class ReportFileTypeExtensionAttributeTest
	{
		[TestCase(ReportFileType.Html, "html")]
		[TestCase(ReportFileType.Pdf, "pdf")]
		public void ShouldGetReportFileExtension(ReportFileType reportFileType, string extension)
		{
			var result = ReportFileTypeExtensionAttribute.Get(reportFileType);

			result.ShouldBeEquivalentTo(extension);
		}
	}
}