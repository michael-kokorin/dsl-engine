namespace Workflow.Event.Handlers
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity.Configuration.ConfigurationHelpers;

	using Newtonsoft.Json;

	using Common.Logging;
	using Infrastructure.Events;
	using Infrastructure.RequestHandling;
	using Infrastructure.RequestHandling.Contracts;
	using Workflow.Properties;

	[UsedImplicitly]
	internal sealed class ExternalSystemActionEventHandler: IEventHandler
	{
		private readonly ILog _log;

		private readonly IRequestExecutorProvider _requestExecutorProvider;

		public ExternalSystemActionEventHandler(
			[NotNull] IRequestExecutorProvider requestExecutorProvider,
			[NotNull] ILog log)
		{
			if(requestExecutorProvider == null) throw new ArgumentNullException(nameof(requestExecutorProvider));
			if(log == null) throw new ArgumentNullException(nameof(log));

			_requestExecutorProvider = requestExecutorProvider;
			_log = log;
		}

		/// <summary>
		///   Determines whether this instance can handle the specified event to handle.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		/// <returns><see langword="true"/> if this instance can handle the event; otherwise, <see langword="false"/>.</returns>
		public bool CanHandle(Event eventToHandle) => eventToHandle.Key == EventKeys.ExternalSystemAction;

		/// <summary>
		///   Handles the specified event to handle.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		public void Handle(Event eventToHandle)
		{
			var request = JsonConvert.DeserializeObject<ApiRequest>(eventToHandle.Data.GetOrNull("Data"));
			if(request == null)
				return;

			var executor = _requestExecutorProvider.Get(request.SourceType);
			try
			{
				executor.Execute(request);
			}
			catch(Exception exception)
			{
				_log.Fatal(Resources.ErrorWhileProcessingRequest, exception);
			}
		}
	}
}