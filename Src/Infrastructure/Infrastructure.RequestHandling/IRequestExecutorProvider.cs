namespace Infrastructure.RequestHandling
{
	/// <summary>
	///   Provides methods to get executor for request.
	/// </summary>
	public interface IRequestExecutorProvider
	{
		/// <summary>
		///   Gets executor for the specified source type and request method.
		/// </summary>
		/// <param name="sourceType">Type of the source.</param>
		/// <returns>The request executor.</returns>
		IRequestExecutor Get(string sourceType);
	}
}