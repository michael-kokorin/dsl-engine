namespace Infrastructure.Reports
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Security;
	using Repository.Context;

	using Authorities = Common.Security.Authorities;

	[UsedImplicitly]
	internal sealed class ReportAuthorityValidator : IReportAuthorityValidator
	{
		private readonly IUserAuthorityValidator _userAuthorityValidator;

		public ReportAuthorityValidator([NotNull] IUserAuthorityValidator userAuthorityValidator)
		{
			if (userAuthorityValidator == null) throw new ArgumentNullException(nameof(userAuthorityValidator));

			_userAuthorityValidator = userAuthorityValidator;
		}

		public bool CanCreate(long userId, long? projectId) => _userAuthorityValidator
			.HasUserAuthorities(userId, new[] {Authorities.UI.Reports.Create}, projectId);

		public bool CanEdit(long userId, Reports report)
		{
			const string authorityKey = Authorities.UI.Reports.Edit;

			return ValidateReportAuthority(userId, report, authorityKey);
		}

		private bool ValidateReportAuthority(long userId, Reports report, string authorityKey)
		{
			var isHasAuthotiryInProject = _userAuthorityValidator.HasUserAuthorities(userId,
				new[] {authorityKey},
				report.ProjectId);

			if (isHasAuthotiryInProject)
				return true;

			var isCanViewSystemReports =
				report.IsSystem &&
				report.ProjectId == null &&
				_userAuthorityValidator.GetProjects(userId, new[] {authorityKey}).Any();

			return isCanViewSystemReports;
		}

		public bool CanRun(long userId, Reports report)
		{
			const string authorityKey = Authorities.UI.Reports.Run;

			return ValidateReportAuthority(userId, report, authorityKey);
		}

		public bool CanView(long userId, Reports report)
		{
			const string authorityKey = Authorities.UI.Reports.View;

			return ValidateReportAuthority(userId, report, authorityKey);
		}

		public IEnumerable<long> GetProjects(long userId, ReportAccessType reportAccessType)
		{
			string authorityName;

			switch (reportAccessType)
			{
				case ReportAccessType.Create:
					authorityName = Authorities.UI.Reports.Create;
					break;

				case ReportAccessType.Edit:
					authorityName = Authorities.UI.Reports.Edit;
					break;

				case ReportAccessType.Run:
					authorityName = Authorities.UI.Reports.Run;
					break;

				case ReportAccessType.View:
					authorityName = Authorities.UI.Reports.View;
					break;

				default:
					throw new InvalidEnumArgumentException();
			}

			return _userAuthorityValidator.GetProjects(userId, new[] {authorityName});
		}
	}
}