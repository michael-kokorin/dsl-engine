namespace Modules.Core.Contracts.Query
{
	using System.ServiceModel;
	using System.ServiceModel.Web;

	using Modules.Core.Contracts.Dto;
	using Modules.Core.Contracts.Query.Dto;
	using Modules.Core.Contracts.UI.Dto.Data;

	[ServiceContract]
	public interface IQueryService
	{
		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST",
			UriTemplate = "/Queries/Create?projectId={projectId}&name={name}")]
		QueryDto Create(long projectId, string name);

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET",
			UriTemplate = "/Queries/Execute?queryId={queryId}&userId={userId}&parameters={parameters}")]
		TableDto Execute(long queryId, long userId, string parameters);

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET",
			UriTemplate = "/Queries/?queryId={queryId}")]
		QueryDto Get(long queryId);

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET",
			UriTemplate = "/DataSources/?projectId={projectId}")]
		DataSourceDto[] GetDataSources(string projectId);

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET",
			UriTemplate = "/DataSources/Fields?dataSourceKey={dataSourceKey}&projectId={projectId}")]
		DataSourceFieldDto[] GetDataSourceFields(string dataSourceKey, string projectId);

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET",
			UriTemplate = "/Queries/List")]
		TableDto GetList();

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET",
			UriTemplate = "/Queries/Privacy")]
		ReferenceItemDto[] GetPrivacyReference();

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET",
			UriTemplate = "/Queries/SortDirections")]
		ReferenceItemDto[] GetSortDirections();

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET",
			UriTemplate = "/Queries/Filter/Conditions")]
		ReferenceItemDto[] GetFilterConditions();

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET",
			UriTemplate = "/Queries/Filter/Operations")]
		ReferenceItemDto[] GetFilterOperations();

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET",
			UriTemplate = "/Queries/CanEdit?queryId={queryId}")]
		bool IsCanEdit(long queryId);

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST",
			UriTemplate = "/Queries/Update")]
		QueryDto Update(QueryDto query);

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST",
			UriTemplate = "/Queries/Convert/Text")]
		string ConvertToText(string queryModel);

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST",
			UriTemplate = "/Queries/Convert/Model")]
		string ConvertToModel(string query);
	}
}