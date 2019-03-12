namespace Infrastructure.Reports.Translation
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Common.Enums;

	[UsedImplicitly]
	internal sealed class ReportTranslatorFabric : IReportTranslatorFabric
	{
		private readonly IUnityContainer _unityContainer;

		public ReportTranslatorFabric([NotNull] IUnityContainer unityContainer)
		{
			if (unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));

			_unityContainer = unityContainer;
		}

		public IReportTranslator GetTranslator(ReportFileType reportFileType)
		{
			var reportFileTypeString = reportFileType.ToString();

			var reportTranslator = _unityContainer.Resolve<IReportTranslator>(reportFileTypeString);

			return reportTranslator;
		}
	}
}