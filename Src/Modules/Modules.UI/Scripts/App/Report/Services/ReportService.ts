///<reference path="../Interfaces/IReportService.ts"/>

module Report.Services {

    export class ReportService implements Interfaces.IReportService {

        httpService: ng.IHttpService;

        windowService: ng.IWindowService;

        constructor($http: ng.IHttpService, $window: ng.IWindowService)
        {
            this.httpService = $http;
            this.windowService = $window;
        }

        getReport(reportId: number, callback: Function, errorCallback: Function)
        {
            var request = this.httpService.get("/report/get", {
                params: {
                    reportId: reportId
                }
            });

            request
                .success((data) =>
                {
                    callback(data);
                })
                .error((response) =>
                {
                    errorCallback(response);
                });
        }

        openReport(reportId: number, parameters: string, type: number)
        {
            this.windowService.open("/report/execute?reportId=" + reportId + "&parameters=" + parameters + "&type=" + type,
                "_blank");
        }

        runReport(reportId: number, parameters: string, callback: Function, errorCallback: Function)
        {
            var request = this.httpService.get("/report/build", {
                params: {
                    reportId: reportId,
                    parameters: parameters
                }
            });

            request
                .success((data) => {
                    callback(data);
                })
                .error((response) => {
                    errorCallback(response);
                });
        }
    }

    angular.module("Scripts.App.Report").service("Report.Services.ReportService", ReportService);
}