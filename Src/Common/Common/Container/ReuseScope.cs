namespace Common.Container
{
	/// <summary>
	///   Container reuse scopes that supported in the system
	/// </summary>
	public enum ReuseScope
	{
		Container = 0,

		Hierarchy = 1,

		PerRequest = 2,

		PerResolve = 3,

		PerThread = 4,

		External = 5
	}
}