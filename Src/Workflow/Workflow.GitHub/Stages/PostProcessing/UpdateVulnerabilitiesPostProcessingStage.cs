namespace Workflow.GitHub.Stages.PostProcessing
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class UpdateVulnerabilitiesPostProcessingStage : PostProcessingStage
	{
		protected override Func<Tasks, bool> PreCondition => _ => true;

		protected override void ExecuteStage(PostProcessingBundle bundle) =>
			bundle.VulnerabilitiesInfo = bundle.VulnerabilitiesInfo
				.GroupBy(x => new {x.Place, x.Type})
				.Select(
					x =>
						x.Any(y => !string.IsNullOrWhiteSpace(y.Exploit))
							? x.FirstOrDefault(y => !string.IsNullOrWhiteSpace(y.Exploit))
							: x.First())
				.Select(x =>
				{
					var splittedPlace = x.Place.Split(':');

					string file;

					if (splittedPlace.Length > 2)
					{
						file = string.Join(":", splittedPlace.Take(splittedPlace.Length - 3));

						x.Position = int.Parse(splittedPlace[splittedPlace.Length - 2]);
					}
					else
					{
						file = x.Place;
					}

					file = file.Substring(bundle.Task.FolderPath.Length).Trim('\\');

					x.File = x.SourceFile = file;

					return x;
				}).ToArray();
	}
}