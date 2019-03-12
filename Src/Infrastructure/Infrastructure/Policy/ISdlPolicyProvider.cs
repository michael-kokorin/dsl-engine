namespace Infrastructure.Policy
{
	public interface ISdlPolicyProvider
	{
		void Add(long projectId, string name, string query);
	}
}