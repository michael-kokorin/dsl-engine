namespace Modules.Core.Contracts.Report
{
	using System.ServiceModel;
	using System.ServiceModel.Web;

	using Modules.Core.Contracts.Report.Dto;
	using Modules.Core.Contracts.UI.Dto.Data;

	[ServiceContract]
	public interface IReportService
	{
		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "DELETE",
			UriTemplate = "/Reports?ReportId={reportId}")]
		void Delete(long reportId);

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET",
			UriTemplate = "/Reports/File?ReportId={reportId}&parameters={parameters}&reportFileType={reportFileType}")]
		ReportFileDto Execute(long reportId, string parameters, int reportFileType);

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET",
			UriTemplate = "/Reports?ReportId={reportId}")]
		ReportDto Get(long reportId);

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json, Method = "GET",
			UriTemplate = "/Reports/List")]
		TableDto GetList();

		[OperationContract]
		[WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST",
			UriTemplate = "/Reports/")]
		void Set(ReportDto report);
	}
}