namespace Infrastructure.Reports.Blocks
{
	public interface IQuaryableReportBlock : IReportBlock
	{
		string QueryKey { get; }
	}
}