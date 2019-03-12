namespace Infrastructure.Reports.Generation.Stages
{
	using System.Linq;

	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class ValidateParametersStage : ReportGenerationStage
	{
		protected override void ExecuteStage(ReportBundle reportBundle)
		{
			if (reportBundle.Rule.Parameters == null)
				reportBundle.Rule.Parameters = new ReportParameter[0];

			if (!reportBundle.Rule.Parameters.Any())
				return;

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var requiredParameter in reportBundle.Rule.Parameters)
			{
				if (!reportBundle.ParameterValues.Any(_ => _.Key.Equals(requiredParameter.Key)))
					throw new RequiredParameterDoesNotSpecifiedException(requiredParameter.Key);
			}
		}
	}
}