namespace Infrastructure.Reports.Blocks
{
	public interface IReportBlockVizualizerFabric
	{
		IReportBlockVizualizer<T> Get<T>(T block) where T : class, IReportBlock;
	}
}