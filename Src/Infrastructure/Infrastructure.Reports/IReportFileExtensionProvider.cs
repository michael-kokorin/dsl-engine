namespace Infrastructure.Reports
{
	using System.Diagnostics.CodeAnalysis;

	using Common.Enums;

	[SuppressMessage("ReSharper", "MemberCanBeInternal")]
	public interface IReportFileExtensionProvider
	{
		string Get(ReportFileType reportFileType);
	}
}