namespace Infrastructure.Reports
{
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;

	using Repository.Context;

	[SuppressMessage("ReSharper", "MemberCanBeInternal")]
	public interface IReportAuthorityValidator
	{
		bool CanCreate(long userId, long? projectId);

		bool CanEdit(long userId, Reports report);

		bool CanRun(long userId, Reports report);

		bool CanView(long userId, Reports report);

		IEnumerable<long> GetProjects(long userId, ReportAccessType reportAccessType);
	}

	public enum ReportAccessType
	{
		Create,

		Edit,

		Run,

		View
	}
}