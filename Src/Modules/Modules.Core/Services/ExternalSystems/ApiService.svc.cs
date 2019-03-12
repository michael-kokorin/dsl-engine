namespace Modules.Core.Services.ExternalSystems
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Newtonsoft.Json;

	using Common.Transaction;
	using Infrastructure.Events;
	using Infrastructure.RequestHandling;
	using Infrastructure.RequestHandling.Contracts;
	using Modules.Core.Contracts.ExternalSystems;
	using Modules.Core.Properties;

	public sealed class ApiService: IApiService
	{
		private readonly IEventProvider _eventProvider;

		private readonly IRequestExecutorProvider _requestExecutorProvider;

		private readonly IUnitOfWork _unitOfWork;

		public ApiService(
			IEventProvider eventProvider,
			IRequestExecutorProvider requestExecutorProvider,
			IUnitOfWork unitOfWork)
		{
			_eventProvider = eventProvider;
			_requestExecutorProvider = requestExecutorProvider;
			_unitOfWork = unitOfWork;
		}

		/// <summary>
		///     Handles the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The result of request execution.</returns>
		public ApiResponse Handle([NotNull] ApiRequest request)
		{
			if (request == null) throw new ArgumentNullException(nameof(request));

			if (string.IsNullOrWhiteSpace(request.SourceType))
			{
				throw new ArgumentException(Resources.ApiService_Handle_SourceTypeNotSpecified);
			}

			if (string.IsNullOrWhiteSpace(request.RequestMethod))
			{
				throw new ArgumentException(Resources.ApiService_Handle_RequestMethodNotSpecified);
			}

			var executor = _requestExecutorProvider.Get(request.SourceType);
			if (!executor.CanHandleAsync(request)) return executor.Execute(request);

			_eventProvider.Publish(
				new Event
				{
					Key = EventKeys.ExternalSystemAction,
					Data = new Dictionary<string, string>
					{
						{
							"Data", JsonConvert.SerializeObject(request)
						}
					}
				});

			_unitOfWork.Commit();

			return new ApiResponse
			{
				Success = true
			};
		}
	}
}