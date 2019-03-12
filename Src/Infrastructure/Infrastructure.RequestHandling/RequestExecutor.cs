namespace Infrastructure.RequestHandling
{
	using System;

	using JetBrains.Annotations;

	using Newtonsoft.Json;

	using Common.Logging;
	using Infrastructure.RequestHandling.Contracts;

	/// <summary>
	///   Represents base class for request executor.
	/// </summary>
	/// <seealso cref="Infrastructure.RequestHandling.IRequestExecutor"/>
	public abstract class RequestExecutor: IRequestExecutor
	{
		/// <summary>
		///   The logger.
		/// </summary>
		protected readonly ILog Logger;

		/// <summary>
		///   Initializes a new instance of the <see cref="RequestExecutor"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="logger"/> is <see langword="null"/>.</exception>
		protected RequestExecutor([NotNull] ILog logger)
		{
			if(logger == null) throw new ArgumentNullException(nameof(logger));

			Logger = logger;
		}

		/// <summary>
		///   Determines whether this instance the specified request can be handled asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns><see langword="true"/> if the request can be handled async; otherwise, <see langword="false"/>.</returns>
		public virtual bool CanHandleAsync(ApiRequest request) => false;

		/// <summary>
		///   Executes the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The request execution result.</returns>
		public virtual ApiResponse Execute(ApiRequest request)
		{
			try
			{
				var result = HandleRequest(request);
				return new ApiResponse
							{
								Success = true,
								JsonData = JsonConvert.SerializeObject(result)
							};
			}
			catch(Exception exception)
			{
				Logger.Error("Execute request", exception);
				return new ApiResponse
							{
								Success = false,
								Message = "An error occurred while executing request"
							};
			}
		}

		/// <summary>
		///   Handles the request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The request execution result.</returns>
		public virtual object HandleRequest(ApiRequest request) => null;
	}
}