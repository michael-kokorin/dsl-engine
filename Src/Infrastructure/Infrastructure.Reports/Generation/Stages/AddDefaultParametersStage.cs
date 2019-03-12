namespace Infrastructure.Reports.Generation.Stages
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.SystemComponents;
	using Common.Time;
	using Infrastructure.UserInterface;

	[UsedImplicitly]
	internal sealed class AddDefaultParametersStage : ReportGenerationStage
	{
		private readonly ISystemVersionProvider _systemVersionProvider;

		private readonly ITimeService _timeService;

		private readonly IUserInterfaceProvider _userInterfaceProvider;

		public AddDefaultParametersStage(
			[NotNull] ISystemVersionProvider systemVersionProvider,
			[NotNull] ITimeService timeService,
			[NotNull] IUserInterfaceProvider userInterfaceProvider)
		{
			if (systemVersionProvider == null) throw new ArgumentNullException(nameof(systemVersionProvider));
			if (timeService == null) throw new ArgumentNullException(nameof(timeService));
			if (userInterfaceProvider == null) throw new ArgumentNullException(nameof(userInterfaceProvider));
			_systemVersionProvider = systemVersionProvider;
			_timeService = timeService;
			_userInterfaceProvider = userInterfaceProvider;
		}

		protected override void ExecuteStage([NotNull] ReportBundle reportBundle)
		{
			if (reportBundle == null) throw new ArgumentNullException(nameof(reportBundle));

			var parameterValues = reportBundle.ParameterValues?.ToDictionary(_ => _.Key, _ => _.Value)
			                      ?? new Dictionary<string, object>();

			parameterValues.Add(DefaultReportParameters.CurrentDate, _timeService.GetUtc().ToString(CultureInfo.CurrentCulture));

			parameterValues.Add(DefaultReportParameters.ReportName, reportBundle.Report.DisplayName);

			parameterValues.Add(DefaultReportParameters.ReportTitle, reportBundle.Title);

			parameterValues.Add(DefaultReportParameters.SystemVersion, _systemVersionProvider.GetSystemVersion());

			parameterValues.Add(DefaultReportParameters.UiHost, _userInterfaceProvider.GetLatest().Host);

			reportBundle.ParameterValues = parameterValues;
		}
	}
}