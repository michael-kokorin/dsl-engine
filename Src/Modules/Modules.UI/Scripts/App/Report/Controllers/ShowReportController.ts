/// <reference path="../../../typings/angularjs/angular.d.ts"/>
/// <reference path="../../../typings/angularjs/angular-sanitize.d.ts" />
/// <reference path="../Interfaces/IReportService.ts"/>
/// <reference path="../Services/ReportService.ts"/>

module Report.Controller {

    export class ShowReportController {

        report: any;

        reportBody: string;

        rule: any;

        reportService: Interfaces.IReportService;

        sceService: ng.ISCEService;

        static $inject = ["Report.Services.ReportService", "$sce"];

        constructor(reportService: Interfaces.IReportService, $sce: ng.ISCEService)
        {
            this.reportService = reportService;

            this.sceService = $sce;
        }

        init(reportId: number)
        {
            console.log("Report Id = " + reportId);
            
            this.reportService.getReport(reportId, this.onReportReceived.bind(this), null);
        }

        onReportReceived(report: any)
        {
            this.report = report;

            this.rule = JSON.parse(this.report.Rule);

            console.log("Repo received. Name=" + this.report.Name);
        }

        run()
        {
            this.reportBody = null;

            let string = null;

            if (this.rule.Parameters != null && this.rule.Parameters.$values != null)
            {
                const parameters = this.rule
                    .Parameters
                    .$values
                    .map(p =>
                    {
                        return {
                            Key: p.Key,
                            Value: p.Value
                        }
                    });

                string = JSON.stringify(parameters);
            }

            this.reportService.runReport(this.report.Id, string, this.onReportBuilt.bind(this), null);

            console.log("Run report");
        }

        onReportBuilt(report: string)
        {
            this.reportBody = this.sceService.trustAsHtml(report);

            console.log("Report built");
        }

        openReport(type: number)
        {
            let string = null;

            if (this.rule.Parameters != null && this.rule.Parameters.$values != null)
            {
                const parameters = this.rule
                    .Parameters
                    .$values
                    .map(p =>
                    {
                        return {
                            Key: p.Key,
                            Value: p.Value
                        }
                    });

                string = JSON.stringify(parameters);
            }

            this.reportService.openReport(
                this.report.Id,
                string,
                type);

            console.log("Report prepared");
        }

        save(type: number)
        {
            this.openReport(type);

            console.log("Save report");
        }
    }

    angular.module("Scripts.App.Report").controller("Report.Controllers.ShowReportController", ShowReportController);
}