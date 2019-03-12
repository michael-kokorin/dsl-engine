/// <reference path="../../../typings/angularjs/angular.d.ts"/>
/// <reference path="../../../typings/angularjs/angular-sanitize.d.ts" />
/// <reference path="../Interfaces/IReportService.ts"/>
/// <reference path="../Services/ReportService.ts"/>
var Report;
(function (Report) {
    var Controller;
    (function (Controller) {
        var ShowReportController = (function () {
            function ShowReportController(reportService, $sce) {
                this.reportService = reportService;
                this.sceService = $sce;
            }
            ShowReportController.prototype.init = function (reportId) {
                console.log("Report Id = " + reportId);
                this.reportService.getReport(reportId, this.onReportReceived.bind(this), null);
            };
            ShowReportController.prototype.onReportReceived = function (report) {
                this.report = report;
                this.rule = JSON.parse(this.report.Rule);
                console.log("Repo received. Name=" + this.report.Name);
            };
            ShowReportController.prototype.run = function () {
                this.reportBody = null;
                var string = null;
                if (this.rule.Parameters != null && this.rule.Parameters.$values != null) {
                    var parameters = this.rule
                        .Parameters
                        .$values
                        .map(function (p) {
                        return {
                            Key: p.Key,
                            Value: p.Value
                        };
                    });
                    string = JSON.stringify(parameters);
                }
                this.reportService.runReport(this.report.Id, string, this.onReportBuilt.bind(this), null);
                console.log("Run report");
            };
            ShowReportController.prototype.onReportBuilt = function (report) {
                this.reportBody = this.sceService.trustAsHtml(report);
                console.log("Report built");
            };
            ShowReportController.prototype.openReport = function (type) {
                var string = null;
                if (this.rule.Parameters != null && this.rule.Parameters.$values != null) {
                    var parameters = this.rule
                        .Parameters
                        .$values
                        .map(function (p) {
                        return {
                            Key: p.Key,
                            Value: p.Value
                        };
                    });
                    string = JSON.stringify(parameters);
                }
                this.reportService.openReport(this.report.Id, string, type);
                console.log("Report prepared");
            };
            ShowReportController.prototype.save = function (type) {
                this.openReport(type);
                console.log("Save report");
            };
            ShowReportController.$inject = ["Report.Services.ReportService", "$sce"];
            return ShowReportController;
        }());
        Controller.ShowReportController = ShowReportController;
        angular.module("Scripts.App.Report").controller("Report.Controllers.ShowReportController", ShowReportController);
    })(Controller = Report.Controller || (Report.Controller = {}));
})(Report || (Report = {}));
//# sourceMappingURL=ShowReportController.js.map