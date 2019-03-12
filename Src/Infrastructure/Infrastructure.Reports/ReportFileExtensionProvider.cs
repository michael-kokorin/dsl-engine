namespace Infrastructure.Reports
{
	using JetBrains.Annotations;

	using Common.Enums;

	[UsedImplicitly]
	internal sealed class ReportFileExtensionProvider : IReportFileExtensionProvider
	{
		public string Get(ReportFileType reportFileType)
		{
			var fileExtension = ReportFileTypeExtensionAttribute.Get(reportFileType);

			return fileExtension;
		}
	}
}