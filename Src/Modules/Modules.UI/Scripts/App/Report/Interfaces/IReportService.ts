module Report.Interfaces {

    export interface IReportService {

        getReport(reportId: number, callback: Function, errorCallback: Function);

        runReport(reportId: number, parameters: string, callback: Function, errorCallback: Function);

        openReport(reportId: number, parameters: string, type: number);
    }

}