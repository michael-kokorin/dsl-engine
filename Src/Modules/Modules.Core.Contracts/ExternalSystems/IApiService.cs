namespace Modules.Core.Contracts.ExternalSystems
{
	using System.ServiceModel;
	using System.ServiceModel.Web;

	using Infrastructure.RequestHandling.Contracts;

	/// <summary>
	///   Provides service methods for external systems API.
	/// </summary>
	[ServiceContract]
	public interface IApiService
	{
		/// <summary>
		///   Handles the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The result of request execution.</returns>
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Handle")]
		ApiResponse Handle(ApiRequest request);
	}
}