///<reference path="../Interfaces/IReportService.ts"/>
var Report;
(function (Report) {
    var Services;
    (function (Services) {
        var ReportService = (function () {
            function ReportService($http, $window) {
                this.httpService = $http;
                this.windowService = $window;
            }
            ReportService.prototype.getReport = function (reportId, callback, errorCallback) {
                var request = this.httpService.get("/report/get", {
                    params: {
                        reportId: reportId
                    }
                });
                request
                    .success(function (data) {
                    callback(data);
                })
                    .error(function (response) {
                    errorCallback(response);
                });
            };
            ReportService.prototype.openReport = function (reportId, parameters, type) {
                this.windowService.open("/report/execute?reportId=" + reportId + "&parameters=" + parameters + "&type=" + type, "_blank");
            };
            ReportService.prototype.runReport = function (reportId, parameters, callback, errorCallback) {
                var request = this.httpService.get("/report/build", {
                    params: {
                        reportId: reportId,
                        parameters: parameters
                    }
                });
                request
                    .success(function (data) {
                    callback(data);
                })
                    .error(function (response) {
                    errorCallback(response);
                });
            };
            return ReportService;
        }());
        Services.ReportService = ReportService;
        angular.module("Scripts.App.Report").service("Report.Services.ReportService", ReportService);
    })(Services = Report.Services || (Report.Services = {}));
})(Report || (Report = {}));
//# sourceMappingURL=ReportService.js.map