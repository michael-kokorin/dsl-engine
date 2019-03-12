namespace Infrastructure.Reports.Blocks.Chart
{
	using JetBrains.Annotations;

	using Common.FileSystem;

	[UsedImplicitly]
	internal sealed class ResourceChartScriptProvider : IChartScriptProvider
	{
		public string GetScript() => FileLoader.FromResource($"{GetType().Namespace}.Chart.js");
	}
}