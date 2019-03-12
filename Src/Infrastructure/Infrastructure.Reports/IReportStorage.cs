namespace Infrastructure.Reports
{
	using System.Linq;

	using Repository.Context;

	public interface IReportStorage
	{
		long Add(long? projectId, string name, string desc, string rule, bool isSystem = false);

		void Delete(long reportId);

		IQueryable<Reports> GetUserQuery();

		Reports Get(long reportId);

		Reports Update(long reportId, string name, string desc, string rule);
	}
}