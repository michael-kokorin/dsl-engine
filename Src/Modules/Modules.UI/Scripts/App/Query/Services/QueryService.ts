/// <reference path="../Interfaces/IQueryService.ts"/>

module Query.Services {
    
    export class QueryService implements Interfaces.IQueryService {

        httpService: ng.IHttpService;

        constructor($http: ng.IHttpService)
        {
            this.httpService = $http;
        }

        getList = (callback: Function) =>
        {
            var result = this.httpService.get("/query/getqueries");

            result.success(data => {
                var arrayResult = <Array<any>>data;

                callback(arrayResult);
            });
        };

        getQuery = (queryId: number, callback: Function, errCallback: Function) => {
            var request = this.httpService.get("/query/get", {
                params: {
                    queryId: queryId
                }
            });

            request
                .success((data) => {
                    callback(data);
                })
                .error((response) => {
                    errCallback(response);
                });
        };

        save = (query, successClbk: Function, errClbk: Function) =>
        {
            var request = this.httpService.post("/query/Update", {query: query});

            request
                .success((data) =>
                {
                    successClbk(data);
                })
                .error((response) =>
                {
                    errClbk(response);
                });
        }

        execute = (queryId: number, userId: number, parameters: any, callback: Function, errCallback: Function) =>
        {
            var request = this.httpService.get("/query/Execute", {
                params: {
                    queryId: queryId,
                    userId: userId,
                    parameters: parameters
                }
            });

            request
                .success((data) => {
                    callback(data);
                })
                .error((response) => {
                    errCallback(response);
                });
        }

        getDataSources(projectId: number, callback: Function, errCallback: Function): void
        {
            var request = this.httpService.get("/query/GetDataSources", {
                params: {
                    projectId: projectId
                }
            });

            request
                .success((data) => {
                    callback(data);
                })
                .error((response) => {
                    errCallback(response);
                });
        }

        getDataSourceFields(dataSourceKey: string, projectId: number, callback: Function, errCallback: Function)
        {
            var request = this.httpService.get("/query/GetDataSourceFields", {
                params: {
                    dataSourceKey: dataSourceKey,
                    projectId: projectId
                }
            });

            request
                .success((data) => {
                    callback(data);
                })
                .error((response) => {
                    errCallback(response);
                });
        }

        getTextQuery(model: string, callback: Function, errCallback: Function)
        {
            var request = this.httpService.post("/query/GetText", { queryModel: model });

            request
                .success((data) =>
                {
                    callback(data);
                })
                .error((response) =>
                {
                    errCallback(response);
                });
        }

        getModelQuery(query: string, callback: Function, errCallback: Function)
        {
            var request = this.httpService.post("/query/GetModel", { query: query });

            request
                .success((data) => {
                    callback(data);
                })
                .error((response) => {
                    errCallback(response);
                });
        }

        getFilterConditions(callback: Function, errCallback: Function): void
        {
            var request = this.httpService.get("/query/GetFilterConditions", { });

            request
                .success((data) => {
                    callback(data);
                })
                .error((response) => {
                    errCallback(response);
                });
        }

        getFilterOperations(callback: Function, errCallback: Function): void
        {
            var request = this.httpService.get("/query/GetFilterOperations", {});

            request
                .success((data) => {
                    callback(data);
                })
                .error((response) => {
                    errCallback(response);
                });
        }
    }

    angular.module("Scripts.App.Query").service("Query.Service.QueryService", QueryService);
}