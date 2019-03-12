namespace Infrastructure.Reports
{
	using System;

	using JetBrains.Annotations;

	using Common.FileSystem;

	[UsedImplicitly]
	internal sealed class ReportFileStorage : IReportFileStorage
	{
		private readonly IFileWriter _fileWriter;

		private readonly IReportFolderPathProvider _reportFolderPathProvider;

		public ReportFileStorage([NotNull] IFileWriter fileWriter, [NotNull] IReportFolderPathProvider reportFolderPathProvider)
		{
			if (fileWriter == null) throw new ArgumentNullException(nameof(fileWriter));
			if (reportFolderPathProvider == null) throw new ArgumentNullException(nameof(reportFolderPathProvider));

			_fileWriter = fileWriter;
			_reportFolderPathProvider = reportFolderPathProvider;
		}

		public void SaveReportFile(ReportBundle reportBundle, [NotNull] ReportFile reportFile, long userId)
		{
			if (reportFile == null) throw new ArgumentNullException(nameof(reportFile));

			var reportFolderPath = _reportFolderPathProvider.GetReportFolderPath(reportBundle, userId);

			_fileWriter.Write(reportFolderPath, reportFile.FileName, reportFile.Content);
		}
	}
}