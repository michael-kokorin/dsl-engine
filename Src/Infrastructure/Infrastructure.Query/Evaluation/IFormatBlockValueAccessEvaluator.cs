namespace Infrastructure.Query.Evaluation
{
	using Infrastructure.DataSource;

	// ReSharper disable once MemberCanBeInternal
	public interface IFormatBlockValueAccessEvaluator
	{
		bool IsAccessible(string value, string dataSourceName, long userId, out DataSourceFieldInfo fieldName);
	}
}