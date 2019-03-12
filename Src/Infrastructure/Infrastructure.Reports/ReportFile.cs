namespace Infrastructure.Reports
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;

	public sealed class ReportFile
	{
		public byte[] Content { get; private set; }

		public string FileName { get; private set; }

		public string RawHtml { get; set; }

		public ReportFileType ReportFileType { get; private set; }

		public string Title { get; set; }

		public ReportFile(
			[NotNull] string title,
			[NotNull] string fileName,
			[NotNull] string rawHtml,
			[NotNull] byte[] content,
			ReportFileType reportFileType)
		{
			if (title == null) throw new ArgumentNullException(nameof(title));
			if (content == null) throw new ArgumentNullException(nameof(content));

			if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));

			FileName = fileName;

			Content = content.ToArray();

			RawHtml = rawHtml;

			ReportFileType = reportFileType;

			Title = title;
		}
	}
}