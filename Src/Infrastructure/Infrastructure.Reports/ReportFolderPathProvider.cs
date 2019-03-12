namespace Infrastructure.Reports
{
	using System;
	using System.IO;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class ReportFolderPathProvider : IReportFolderPathProvider
	{
		private readonly IReportFolderPathStorage _reportFolderPathStorage;

		private readonly IUserRepository _userRepository;

		public ReportFolderPathProvider([NotNull] IReportFolderPathStorage reportFolderPathStorage,
			[NotNull] IUserRepository userRepository)
		{
			if (reportFolderPathStorage == null) throw new ArgumentNullException(nameof(reportFolderPathStorage));
			if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));

			_reportFolderPathStorage = reportFolderPathStorage;
			_userRepository = userRepository;
		}

		public string GetReportFolderPath([NotNull] ReportBundle reportBundle, long userId)
		{
			if (reportBundle == null) throw new ArgumentNullException(nameof(reportBundle));

			var reportPath = GetReportPath();

			var validReportName = reportBundle.Report.DisplayName.ToValidPath();

			var reportFilePath = Path.Combine(reportPath, validReportName);

			var reportUserLogin = GerReportUserName(userId).ToValidPath();

			reportFilePath = Path.Combine(reportFilePath, reportUserLogin);

			return reportFilePath;
		}

		private string GetReportPath()
		{
			var reportRootDir = _reportFolderPathStorage.GetReportFolderPath();
			return reportRootDir;
		}

		private string GerReportUserName(long userId)
		{
			var reportUser = _userRepository.Get(userId).SingleOrDefault();

			if (reportUser == null)
				throw new ArgumentException(nameof(userId));

			return reportUser.Login;
		}
	}
}