namespace Infrastructure.RequestHandling
{
	using Infrastructure.RequestHandling.Contracts;

	/// <summary>
	///   Provides methods to execute request.
	/// </summary>
	public interface IRequestExecutor
	{
		/// <summary>
		///   Determines whether this instance the specified request can be handled asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns><see langword="true"/> if the request can be handled async; otherwise, <see langword="false"/>.</returns>
		bool CanHandleAsync(ApiRequest request);

		/// <summary>
		///   Executes the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The request execution result.</returns>
		ApiResponse Execute(ApiRequest request);
	}
}